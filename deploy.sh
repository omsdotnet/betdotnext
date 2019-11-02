ls -lR

echo 'decrypt file key'
curl -sflL 'https://raw.githubusercontent.com/appveyor/secure-file/master/install.sh' | bash -e -
./appveyor-tools/secure-file -decrypt keys/key.enc -secret $secret -salt $salt
if [ $? -ne 0 ]; then
    echo 'secure-file fail'
    exit 1
fi

echo 'substitute docker-compose.yml'
envsubst < 'docker-compose.prod.yml' > 'docker-compose.yml'
if [ $? -ne 0 ]; then
    echo 'envsubst fail'
    exit 1
fi

./rdeploy/rdeploy --username root --host $server --private-key /keys/key --passphrase $code --source docker-compose.yml --destination "/home/dotnext/docker-compose.yml"
./rdeploy/rdeploy --username root --host $server --private-key /keys/key --passphrase $code --command "cd /home/dotnext | docker-compose up -d"