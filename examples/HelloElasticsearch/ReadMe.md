![Essential Logging](../../docs/images/diagnostics-logo-64.png)

# HelloElasticsearch

This logger provider writes to Elasticsearch and can be viewed in the Kibana console.

## Running the Example

You need to be running Elasticsearch and Kibana, for example on Linux a docker compose configuration is provided. There are a number 
of prerequesites that you will need to meet, such as enough file handles; the elk-docker project provides a good list, including some troubleshooting 
(see https://elk-docker.readthedocs.io/).

The provided Docker compose will create two nodes, one for Elasticsearch, and one for Kibana:

```powershell
sudo docker-compose -f examples/HelloElasticsearch/docker/docker-compose.yml up
```

Then in another console, run the HelloElasticsearch example:

```powershell
dotnet run --project ./examples/HelloElasticsearch
```

Open a browser to the Kibana application:

```
http://localhost:5601
```

You will be prompted to create a new index pattern, based on the received messages (if it says there are no log messages, you need to troubleshoot the issue).

Use the index pattern "log-*", which matches the events sent, use the time filter "@timestamp", and create the pattern.

On the Discover tab, you will see your log messages, with semantic parameters logged as individual fields. 

You can configure columns as needed, and search for specific fields (e.g. "fields.CustomerId: 12345").

**Example output: Elasticsearch via Kibana** 

![Example - Elasticsearch Kibana](../../docs/images/example-elasticsearch-kibana.png)

You can stop the container with `^C`. You can start it again, running in the background, with:

```powershell
sudo docker-compose -f examples/HelloElasticsearch/docker/docker-compose.yml start
```

## Configuration

Examples of both a custom configuration (`appsettings.json`) is provided.

Full details of the configuration, including the available template arguments, are in the ElasticsearchLoggerProvider project.

* See [ElasticsearchLoggerProvider](../../src/Essential.LoggerProvider.Elasticsearch)
