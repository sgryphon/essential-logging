# Comparison

## Essential, uses Elasticsearch Common Schema (ECS)

```json
{
  "_index": "dotnet-2020.03.26",
  "_type": "_doc",
  "_id": "e17bfee9-8fb9-4de3-ab04-335718f37aa1",
  "_version": 1,
  "_score": null,
  "_source": {
    "agent": {
      "type": "Essential.LoggerProvider.Elasticsearch",
      "version": "0.0.1"
    },
    "ecs": {
      "version": "1.5"
    },
    "error": {
      "message": "Calculation error",
      "stack_trace": "   at HelloElasticsearch.Worker.ExecuteAsync(CancellationToken stoppingToken) in /home/sly/Code/essential-logging/examples/HelloElasticsearch/Worker.cs:line 65\n---> System.DivideByZeroException: Attempted to divide by zero.\n   at HelloElasticsearch.Worker.ExecuteAsync(CancellationToken stoppingToken) in /home/sly/Code/essential-logging/examples/HelloElasticsearch/Worker.cs:line 61",
      "type": "System.Exception"
    },
    "event": {
      "name": "ErrorProcessingCustomer",
      "code": "5000",
      "severity": 3
    },
    "host": {
      "architecture": "X64",
      "hostname": "VUB1804",
      "os": {
        "full": "Linux 4.15.0-91-generic #92-Ubuntu SMP Fri Feb 28 11:09:48 UTC 2020",
        "platform": "Unix",
        "version": "4.15.0.91"
      }
    },
    "labels": {
      "ip": "2001:db8:85a3::8a2e:370:7334",
      "CustomerId": "12345"
    },
    "log": {
      "level": "Error",
      "logger": "HelloElasticsearch.Worker"
    },
    "message": "Unexpected error processing customer 12345.",
    "MessageTemplate": "Unexpected error processing customer {CustomerId}.",
    "process": {
      "name": "HelloElasticsearch",
      "pid": 20273,
      "thread": {
        "id": 6
      }
    },
    "Scopes": [
      "IP address 2001:db8:85a3::8a2e:370:7334",
      "PlainScope"
    ],
    "service": {
      "type": "HelloElasticsearch",
      "version": "1.0.0"
    },
    "tags": [
      "Development"
    ],
    "@timestamp": "2020-03-27T12:53:17.6266621+10:00",
    "user": {
      "domain": "VUB1804",
      "name": "sly"
    }
  },
  "fields": {
    "@timestamp": [
      "2020-03-27T02:53:17.626Z"
    ]
  },
  "sort": [
    1585277597626
  ]
}
```

## Serilog

From local sample program running Serilog.Sinks.ElasticSearch, version 8.0.1.

```json
{
  "_index": "logstash-2020.03.27",
  "_type": "_doc",
  "_id": "oRBcG3EBzd4NX05P6Yzz",
  "_version": 1,
  "_score": null,
  "_source": {
    "@timestamp": "2020-03-27T19:40:04.8413090+10:00",
    "level": "Error",
    "messageTemplate": "Unexpected error processing customer {CustomerId}.",
    "message": "Unexpected error processing customer 12345.",
    "exceptions": [
      {
        "Depth": 0,
        "ClassName": "System.DivideByZeroException",
        "Message": "Attempted to divide by zero.",
        "Source": "ElkStack",
        "StackTraceString": "   at Examples.Worker.ExecuteAsync(CancellationToken stoppingToken) in /home/sly/Code/syslog-structureddata/examples/DefaultConsoleLogging/Worker.cs:line 44",
        "RemoteStackTraceString": null,
        "RemoteStackIndex": 0,
        "HResult": -2147352558,
        "HelpURL": null
      }
    ],
    "fields": {
      "CustomerId": 12345,
      "EventId": {
        "Id": 5000,
        "Name": "ErrorProcessingCustomer"
      },
      "SourceContext": "Examples.Worker",
      "origin:ip": "2001:db8:85a3::8a2e:370:7334",
      "SD-ID": "origin"
    }
  },
  "fields": {
    "@timestamp": [
      "2020-03-27T09:40:04.841Z"
    ]
  },
  "sort": [
    1585302004841
  ]
}
```

## Log4net

Both https://github.com/bruno-garcia/log4net.ElasticSearch and https://github.com/ptylenda/log4net.ElasticSearch.Async have the same example:

```json
{
	"_index": "log-2016.02.12",
	"_type": "logEvent",
	"_id": "AVLXHEwEJfnUYPcgkJ5r",
	"_version": 1,
	"_score": 1,
	"_source": {
		"timeStamp": "2016-02-12T20:11:41.5864254Z",
		"message": "Something broke.",
		"messageObject": {},
		"exception": {
			"Type": "System.Exception",
			"Message": "There was a system error",
			"HelpLink": null,
			"Source": null,
			"HResult": -2146233088,
			"StackTrace": null,
			"Data": {
				"CustomProperty": "CustomPropertyValue",
				"SystemUserID": "User43"
			},
			"InnerException": null
		},
		"loggerName": "log4net.ES.Example.Program",
		"domain": "log4net.ES.Example.vshost.exe",
		"identity": "",
		"level": "ERROR",
		"className": "log4net.ES.Example.Program",
		"fileName": "C:\\Users\\jtoto\\projects\\log4net.ES.Example\\log4net.ES.Example\\Program.cs",
		"lineNumber": "26",
		"fullInfo": "log4net.ES.Example.Program.Main(C:\\Users\\jtoto\\projects\\log4net.ES.Example\\log4net.ES.Example\\Program.cs:26)",
		"methodName": "Main",
		"fix": "LocationInfo, UserName, Identity, Partial",
		"properties": {
			"log4net:Identity": "",
			"log4net:UserName": "JToto",
			"log4net:HostName": "JToto01",
			"@timestamp": "2016-02-12T20:11:41.5864254Z"
		},
		"userName": "JToto",
		"threadName": "9",
		"hostName": "JTOTO01"
	}
}
```
