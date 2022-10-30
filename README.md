# Parts Hole

Inventory manager and parser for [DigiKey](https://www.digikey.com/MyDigiKey) and [Mouser](https://www.mouser.com/MyMouser/AccountSummary.aspx). Designed to take data from both suppliers and convert it to a custom data structure. As of now, it's stored locally in a JSON file. In the future, I want to get an API up and running that can store the data in a MongoDB database.

This is specifically designed for electronic component management. Complex components and sub-components are not planned.

## Features

- Full Inventory management, including Quantities, custom part numbers, and location tracking
- Digikey and Mouser invoice parsing
- Project BOM and versioning Management
- KiCAD BOM parser
- Custom Part Number creation tools
- Passive component (Resistors, Capacitors, Inductors) Management page with standardized value tools