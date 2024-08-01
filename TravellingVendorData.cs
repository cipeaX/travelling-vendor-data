using System;
using Facepunch;
using Rust;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;

namespace Oxide.Plugins
{
    [Info( "TravellingVendorData", "cipeaX", "1.0.0" )]
    public class TravellingVendorData : RustPlugin
    {

        public class vendingMachine
        {
            public string? Prefab;
            public string? DisplayName;
            public List<VendingMachineOrder>? Orders;
        };

        public class VendingMachineOrderItem
        {
            public string? ShortName;
            public int Amount;
            public bool IsBlueprint;
        }

        public class VendingMachineOrder
        {
            public VendingMachineOrderItem? ForSale;
            public VendingMachineOrderItem? Currency;
            public float RefillDelay;
            public VendingMachineOrderRandomDetails? RandomDetails;
        }

        public class VendingMachineOrderRandomDetails
        {
            public float weight;
            public int minPrice;
            public int maxPrice;
            public float veryLowPriceChance;
            public int veryLowPriceMin;
            public int veryLowPriceMax;
        }

        private static JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private static string _travellingVendorDataDirectory = Path.Combine( Directory.GetCurrentDirectory(), "TravellingVendorData" );

        private void Init()
        {
            if( !Directory.Exists( _travellingVendorDataDirectory ) )
            {
                Directory.CreateDirectory( _travellingVendorDataDirectory );
            }
        }

        private void OnServerInitialized()
        {
            var npcVendingMachines = UnityEngine.Object.FindObjectsOfType<NPCVendingMachine>();
            var npcVendingMachineNames = npcVendingMachines.Select(vendingMachine => vendingMachine.Phrase.english).ToArray();

            if ( !npcVendingMachineNames.Contains("Travelling Vendor"))
            {
                Puts("Spawning in Travelling Vendor.");
                server.Command("Travellingvendor.StartEvent");
            }

            npcVendingMachines = UnityEngine.Object.FindObjectsOfType<NPCVendingMachine>();
            var travellingVendor = npcVendingMachines.FirstOrDefault( x => x.Phrase.english == "Travelling Vendor" );

            vendingMachine travellingVendorData = new vendingMachine
            {
                Prefab = travellingVendor?.PrefabName,
                DisplayName = travellingVendor?.Phrase.english,
                Orders = new List<VendingMachineOrder>()
            };

            if (travellingVendor != null)
            {
                foreach( var order in travellingVendor.vendingOrders.orders )
                {
                    var vendingOrder = new VendingMachineOrder
                    {
                        ForSale = new VendingMachineOrderItem
                        {
                            ShortName = order.sellItem.displayName.english,
                            Amount = order.sellItemAmount,
                            IsBlueprint = order.sellItemAsBP
                        },
                        Currency = new VendingMachineOrderItem
                        {
                            ShortName = order.currencyItem.displayName.english,
                            IsBlueprint = order.currencyAsBP
                        },
                        RandomDetails = new VendingMachineOrderRandomDetails
                        {
                             weight = order.randomDetails.weight,
                             minPrice = order.randomDetails.minPrice,
                             maxPrice = order.randomDetails.maxPrice,
                             veryLowPriceChance = order.randomDetails.veryLowPriceChance,
                             veryLowPriceMin = order.randomDetails.veryLowPriceMin,
                             veryLowPriceMax = order.randomDetails.veryLowPriceMax
                        },
                        RefillDelay = order.refillDelay
                    };
                    travellingVendorData.Orders.Add( vendingOrder );
                }

            var jsonPath = Path.Combine( _travellingVendorDataDirectory, "TravellingVendor" + ".json" );
            
            SaveData(travellingVendorData, jsonPath );
            Puts("Successfully saved Travelling Vendor Data.");
            }
            
        }

        private void SaveData( object data, string path )
        {
            var json = JsonConvert.SerializeObject( data, _jsonSettings );
            File.WriteAllText( path, json );
        }
    }
}