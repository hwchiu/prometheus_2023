#!/bin/bash

hostname=`hostname`
for i in $(cat /proc/net/dev | tail -n +3 | awk '{print $1 $2}')
do
  iface=$(echo $i | cut -d ':' -f1)
  rx=$(echo $i | cut -d ':' -f2)
  echo "network_tx_bytes{hostname=\"$hostname\", iface=\"$iface\"} $rx" >> a
done

echo "update to gw"
curl --data-binary "@a" http://localhost:9091/metrics/job/docker/instance/dockerrunning
rm a
