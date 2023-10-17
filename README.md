# nym-mixnode-monitor
Expose Prometheus friendly metrics endpoint for monitoring Nym mixnodes


Run locally in an interactive session whist exposing the metrics at http://localhost:9090/metrics
Note
* -it = interactive session
* --rm - remove the container when completed
* -e MixnodeId= - the mixnodeId of the node to monitor
* -e MixnodeIp= - the ip address of the mixnode to monitor (defaults to 127.0.0.1
* --log-driver local - use the local file logging driver to get the benefits of log file rotation and size limits
* --log-opt max-size - limit the size of the log files
* nym-mixnode-monitor:dev - use the dev tag
```
docker build . --tag=nym-mixnode-monitor:dev
docker run -it --rm -p 9090:80  -e MixnodeId=292 -e MixnodeIp=5.161.43.60 --log-driver local --log-opt max-size=10m nym-mixnode-monitor:dev
```
