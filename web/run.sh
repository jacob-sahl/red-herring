# Create a network, which allows containers to communicate
# with each other, by using their container name as a hostname
docker network create red_herring_network

# Build prod using new BuildKit engine
docker compose -f docker-compose.yml build

# Up prod in detached mode
docker compose -f docker-compose.yml up -d