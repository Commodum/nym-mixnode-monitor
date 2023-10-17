# nym-mixnode-monitor
Expose Prometheus friendly metrics endpoint for monitoring Nym mixnodes

This container calls the Mixnode api and the Nym API to update key telemetry and then expose them as Prometheus metrics on a ```/metrics``` endpoint.  As the mixnode ID and host address are configured this container does not have to be running on the mixnode, it can run anywhere.

## Run locally
Run locally in an interactive session whist exposing the metrics at http://localhost:9090/metrics
```
git pull https://github.com/Commodum/nym-mixnode-monitor.git
cd nym-mixnode-nym-mixnode-monitor
docker build NymMixnetMonitor/ --tag=nym-mixnode-monitor:dev
docker run -it --rm -p 9090:80  -e MixnodeId=5 -e MixnodeIp=185.229.90.217 --log-driver local --log-opt max-size=10m nym-mixnode-monitor:dev
```
### Notes
| param | Description |
| --- | --- |
| -it | interactive session |
| --rm | remove the container when completed/exited |
| -e MixnodeId=5 | Set the MixnodeId environment variable to be the Mix Id of [Swiss Staking's mixnode](https://explorer.nymtech.net/network-components/mixnode/5) |
| -e MixnodeIp=185.229.90.217 | Set the MixnodeIp environment variable to be the host address of [Swiss Staking's mixnode](https://explorer.nymtech.net/network-components/mixnode/5) |
| --log-driver local | use the local file logging driver to get the benefits of log file rotation and size limits |
| --log-opt max-size | limit the size of the log files |

## Run container as systemd service
An example service file is located [here](nym-mixnet-monitor.service).
This service pulls the latest instance of the nym-mixnode-monitor package, runs it and exposes the metrics on port 9090.  This file should be customised to override the ```MixnodeId``` and ```MixnodeIp``` environmental variables to the mixnodeId and IP address of your mixnode instance.  Please note that localhost or 127.0.0.1 cannot be used for the MixnodeIp.

### Environment Variables that can be configured within the .service file
| Environmental Variable | Description | Mandatory | Default |
| --- | --- | --- | --- |
| MixnodeId | 'mix Id' of your mixnode, eg 5 | ✔️| |
| MixnodeIp | Public Ip/Host address of your mixnode, eg 185.229.90.217 | ✔️ | |
| NymApiBaseUrl | Url of the Nym Api | | https://validator.nymtech.net/ |
| MixnodeApiPort | Port where the mixnode api is exposed on your mixnode | | 8000 |
| MixnodeScheme| Scheme which the mixnode api uses | | http |
| Logging__LogLevel__Default | Logging level. Options: Trace, Debug, Information, Warning, Error, Critical & None | | Information |

### Set up steps
* ensure Docker Engine is installed on your target machine. [Doco for Ubuntu is here](https://docs.docker.com/engine/install/ubuntu/).
* create /etc/systemd/system/nym-mixnode-monitor.service
* copy the content from [nym-mixnode-monitor.service](nym-mixnode-monitor.service) into the file
* update the ```-e MixnodeId=``` environment variable to the Mix Id of your node
* update the ``` -e MixnodeIp=``` environment variable to the IP address of your node
* update any further parameters that you see fit
* save the file
* reload the systemctl daemon ```systemctl daemon-reload```
* enable the service (so it starts up on re-boot) ```systemctl enable nym-mixnode-monitor```
* start the service ```systemctl enable nym-mixnode-monitor```
* view metrics ```curl http://localhost:9090/metrics```
* open firewall for port 9090

## Local development environment
This application is written in c# targeting .NET 6.0.  Opening the solution file (NymMixnetMonitor.sln) in Visual Studio 2022 would be the easiest way to get started.
To test against your own Mixnode instance, override the MixnodeId and MixnodeIp settings in the appsettings.Development.json file