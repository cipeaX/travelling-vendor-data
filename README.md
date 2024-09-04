# Travelling Vendor Data

A small Rust Plugin that reads out the Data behind the offers of the Travelling Vendor to a JSON file.

The plugin will first attempt to find the Vendor on the Map, spawning it in if it doesnt exist.
The data will then be saved to the `TravellingVendorData` Folder in the Rust Server Directory.

## Example Output:
```json
{
"prefab": "assets/prefabs/deployable/vendingmachine/npcvendingmachines/npcvendingmachine_travellingvendor.prefab",
  "displayName": "Travelling Vendor",
  "orders": [
    {
      "forSale": {
        "shortName": "Jackhammer",
        "amount": 1,
        "isBlueprint": false
      },
      "currency": {
        "shortName": "Scrap",
        "amount": 0,
        "isBlueprint": false
      },
      "refillDelay": 10.0,
      "randomDetails": {
        "weight": 0.05,
        "minPrice": 80,
        "maxPrice": 120,
        "veryLowPriceChance": 0.2,
        "veryLowPriceMin": 40,
        "veryLowPriceMax": 60
      }
    },
    ...
  ]
}
```
Credit to hoppel / RustHelp for the JSON saving functionality.
