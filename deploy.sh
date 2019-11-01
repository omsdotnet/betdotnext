
ls -lr

echo 'decrypt file key'
./appveyor-tools/secure-file -decrypt /keys/key.enc -secret $secret -salt $salt
if [[ $? -ne 0 ]]; then
echo 'secure-file fail'
exit 1

echo 'substitute docker-compose.yml'
envsubst < 'docker-compose.prod.yml' > 'docker-compose.yml'
if [[ $? -ne 0 ]]; then
echo 'envsubst fail'
exit 1

ssh -i /keys/key root@$server