#!/bin/bash

allname=`docker ps --format "{{.Names}}"`
hostname=`hostname`
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
  echo "dockerrunning{host=\"${hostname}\", name=\"$i\"} $t" >> a
done

curl --data-binary "@a" https://prometheus-gw.hwchiu.com/metrics/job/docker/remote_instance/dockerrunning
rm a
