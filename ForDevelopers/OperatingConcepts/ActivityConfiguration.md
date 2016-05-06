# ACTIVITY CONFIGURATION

[Go to Contents](https://github.com/Fr8org/Fr8Core.NET/blob/master/README.md)  

Activities are configured at Design-Time. Configuration is the responsibility of the Terminal that hosts that Activity. The terminal instructs the Client as to what UI controls should be displayed and as a result the Client doesn�t need to know anything specific about the Activity. Likewise the Hub stays uninvolved with Activity Configuration, serving as a simple conduit to enable the Client and Terminal to exchange data.  

Example: The Write To Azure Sql Server action needs a connection string in order to work. It defines that need in json and passes the json back in response to Configure requests. The client front-end will likely render UI based on this json, showing the user something like this:  

![Write To SQL Server](https://github.com/Fr8org/Fr8Core.NET/blob/master/img/ActivityConfiguration_WriteToSQLServer.png)

This approach has a big benefit: a developer can create a new Action, complete with sophisticated configuration UI, and deploy it without any changes being made to the Hub or Client code. It�s analogous to being able to deploy a web page without having to change any of the web servers or browsers �out there� on the web, so long as you stick to HTML and Javascript standards.  

The complexity of the configuration process depends entirely on the Action. Some Actions require no configuration at all, while others require multiple steps.  

Key Design Concepts  

##  Activities define their UI via a JSON mechanism  

Configuration begins when a Client adds or loads an Activity into a Plan. the Client POSTs to the Hub�s /activities/configure endpoint. The Hub checks to see if the Activity requires authorization, which basically means that its Terminal is going to need to log into a web service. If authorization is required, the Hub checks to see if it is storing a relevant AuthorizationToken. If so, the token is POSTed to the appropriate Terminal�s /actions/configure endpoint along with the Action json. (If a token is not available, the Hub responds to the Client, instructing it to initiate a (usually OAuth) authorization process).  

Fr8 breaks configuration calls into two buckets: Initial and Followup. The Action code evaluates the received request and decides whether the call is an Initial or Followup call, and acts accordingly.  The typical usage pattern is to respond to Initial calls by composing the configuration UI that the user should see, and respond to Followup calls by updating the configuration UI based on the user�s choices. For example, the Monitor DocuSign Action�s Initial Configuration creates a drop down list box populated with the names of all of the user�s DocuSign templates. When the user selects a template, a followup call is made to the Action, which extracts the custom fields from that particular template and adds them to the Action. This enables downstream actions to use those custom fields in their own configuration (�Extract the value of the custom field called Doctor and add it to this document�)    

### Triggering Configuration Requests using Events  

There are a couple of other mechanisms that an Action can use to trigger configure calls back to itself. All of the Configuration Controls can have an Events property, and an Action can set a control to trigger a new configuration call back to itself. For example, in the above example, the Followup Configuration call is created when the Action adds an Event property to the Select Template drop down list box called onSelect which instructs the Client to RequestConfig.  

Through this process, an iterative sequence can be created where the Configuration UI that the user sees keeps updating in response to the user�s selections.  

## Triggering Configuration Requests using the AggressiveReload Tag

The Fr8 Client V1 standard supports the AggressiveReload Tag, which can be added by a Terminal to the ActivityTemplate json of an Activity. When an Activity returns from configuration, the Client will then make followup configuration calls to any Activities that have the AggressiveReload tag. This is useful when the Activity is particularly dependent on upstream and/or downstream data sources, as it ensures that the Activity always updates itself in response to changes in other Activities. Note that most of the time this isn�t necessary because an Action can query for upstream data when it�s Configured.  

![Configure Flow](https://github.com/Fr8org/Fr8Core.NET/blob/master/img/ActivityConfiguration_ConfigureFlow.png)

## Action Configuration Can Use a Set of Defined UI Controls

Learn more about the [UI Control Definitions.](https://github.com/Fr8org/Fr8Core.NET/blob/master/ForDevelopers/DevelopmentGuides/ConfigurationControls.md)

[Go to Contents](https://github.com/Fr8org/Fr8Core.NET/blob/master/README.md)  