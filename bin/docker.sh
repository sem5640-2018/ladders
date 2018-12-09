echo "$DOCKERHUB_PASSWORD" | docker login -u "$DOCKERHUB_USERNAME" --password-stdin
cd ladders
docker build -t sem56402018/ladders:$1 -t sem56402018/ladders:$TRAVIS_COMMIT .
docker push sem56402018/ladders:$TRAVIS_COMMIT
docker push sem56402018/ladders:$1
