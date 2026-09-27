# BakeCatch

Repository for the BakeCatch physical game project.

## Project areas

- `web-dashboard/` — Firebase dashboard
- `unity-game/` — Unity game
- `firmware/` — ESP32 and sensor firmware
- `mqtt-bridge/` — MQTT/WebSocket integration
- `firebase/` — Firebase rules and deployment configuration
- `docs/` — diagrams, data contracts, and project notes

## Sensitive files

Do not commit `.env` files, service-account credentials, private keys, or other secrets. The repository ignores common secret and build-output patterns. Review Firebase security rules before making this repository public.
