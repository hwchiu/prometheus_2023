#!/bin/bash

allname=`docker ps --format "{{.Names}}"`
function dockerrunning(){
    containerName=$1
    exist=`docker inspect --format '{{.State.Status}}' ${containerName}`

    if [ "${exist}" != "running" ]; then
        echo 0
    else
        echo 1
    fi
    }

echo "# TYPE dockerrunning  counter" > a
for i in ${allname}
do

  t=`dockerrunning $i`
  echo "dockerrunning{name=\"$i\"} $t" >> a
done


curl --data-binary "@a" http://localhost:9091/metrics/job/docker/instance/dockerrunning
rm a
