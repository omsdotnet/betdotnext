
echo 'decrypt file key'
./appveyor-tools/secure-file -decrypt /keys/key.enc -secret $secret -salt $salt

echo 'substitute docker-compose.yml'
envsubst < 'docker-compose.prod.yml' > 'docker-compose.yml'