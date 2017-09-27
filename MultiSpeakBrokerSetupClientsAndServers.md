# MultiSpeak Broker Setup for Clients and Servers

This document will provide the user with a walk through in setting up the
MultiSpeak Broker.  The goal is to provide a production like example anyone
can follow and setup the broker.

## Automated Meter Infrastructure Systems

AMI systems will never make any requests to the Broker.  An AMI system
will only publish notifications to the Broker which require maybe an AMI
Authorized user to be setup.

AMI systems are usually the reciever of all the requests.

## Customer Information Systems

CIS System (or our own CSR Portal) will be the main users of sending requests to the broker.

### ExampleVendor

Our vendor, who goes by the fancy yet uncreative name of 'FakeCISMSpeak3', and has 50%
of the co-operative market will need to send the following commands to the AMI system.
But ... wait.  There are two AMI systems so we need to have FakeCISMSpeak3 send these
requests to the MutliSpeakBroker which forwards the request to the AMI System.

List of commands the FakeCISMSpeak3 vendor will send to the AMI system

- ServiceLocationChangedNotification
- CancelDisconnectedStatus
- CancelUsageMonitoring
- MeterAddNotification
- MeterChangedNotification
- MeterRemoveNotification
- InitiateUsageMonitoring
- InitiateDisconnectedStatus
- GetAMRSupportedMeters
- GetLatestReadingByMeterNo
- GetCDSupportedMeters
- GetCDMeterState
- InitiateConnectDisconnect

Step 1:
Add FakeCISMSpeak3 to Authorized Users.  That's it move on.

~Screen Shots to Add~

Step 2:
Setup the AMI Vendor, and we can call the vendor... something special like FakeAMI

Vendor Configuration > Add Vendor
Name: Landis & Gyr
Description: ReadSource to send and receive AMI Commands for MSpeak v3 (old version)

Since we setup an AMI Vendor, we also need to setup the MDMWebAPI Settings.

Normally the MDMWebAPI will be on our on Application Server and use the typical user name and passwords.
Check the MDMWebAPI.config for user and passwords... unless we store this somewhere else now.

Remember, we can support multiple AMI Vendors so when we receive a request that will be
sent to an AMI Vendor, we will make a request to the MDMWebAPI to see which AMI Vendor
owns this meter.   How we did this is by using the VendorName which needs to 
match (EXACTLY, cases and spelling) the name of the read source in MDM.dbo.ReadSource.

The Vendor Name is used to generate a query to the MDM to see if this
meter exists in MDM.dbo.Meter either by MeterIdentifier, (Serial) AMISerialNo
or AMIControlIdentifier.

In most cases use MeterIdentifier.  If you get back Meter does not exists or see no meter
number in the request... make sure the AMI Meter Type is correctly setup to the

What is the query we are using?

```sql

        SELECT TOP 1 MeterIdentifier FROM
        MDM.dbo.Meter
        WHERE MeterIdentifier = @MeterIdentifier;

```

OR

```sql

        SELECT TOP 1 AMISerialNo FROM
        MDM.dbo.Meter
        WHERE AMISerialNo = @AMISerialNo;

```

TODO: Go back and get the real query ...

Step 3:
Set Subscribers for all the methods above... that a CIS Vendor will send to the AMI system.

http://10.86.1.81/MultiSpeakBroker/SubscribersConfig.aspx?ReadSource=Landis & Gyr

http://10.86.1.81/MultiSpeakBroker/SubscribersConfig2.aspx?ReadSource=Landis

TODO: Advanced Editor and Normal Editor do not show the same list when looking at the setup endpoint methods.
http://10.86.1.81/MultiSpeakBroker/SubscribersConfig2.aspx?ReadSource=Landis
Is missing al lthe Meter*.* Notifications

http://10.86.1.81/MultiSpeakBroker/SubscribersConfig.aspx?ReadSource=Landis

URL is not handled properly when there are spaces in the Vendor Name.
Landis & Gyr has an Ampersand... this is mess up the data in the database and query?

TODO: Tab always shows 'Waiting for response from $URLOfWebSite
Back to the Topic at hand...

Click Set Subscribers for this Vendor (FakeAMI 0r Landis & Gyr)

Each individual method will need the following.

Example
Find 'MSpeak3 ServiceLocationChangedNotification' on this page http://10.86.1.81/MultiSpeakBroker/SubscribersConfig.aspx?ReadSource=Landis+&+Gyr
FakeCISMSpeak3 will send a a ServiceLocationChangedNotification to the Broker in, what version? MultiSpeak Version 3!! Yes.
So we choose 'MSpeak3 ServiceLocationChangedNotification'

Or AMI Vendor could be using MultiSpeak 3 or MultiSpeak 4... which one is it?  Ask the AMI vendor!

TODO: Rename Client Request to something like Incoming Client Request Method and Version or anything to help a dev out.

Edit Subscriber - ServiceLocationChangedNotification (MSpeak3)
Response Method(MSpeak3):

TODO:  The title in the popup is good but can be better.  The Response Method is really the Host Field parameter.  And If I 
change Host from MSpeak3 to MSpeak4, the ResponseMethod is still (MSpeak3)... Is this valid?? Why is it like this? 

TODO:  The Client Request and Receiver Host, and Task are the columns in this grid on page: SubscribersConfig.aspx
The label for the Edit popup has Host: instead of Receiver Host.

Host:  THIS SHOULD BE RECEIVER HOST, I WILL JUST NOTE THIS AND MOVE ON

When you click Edit you will see the following Urls
Host: is the MultiSpeak Version the AMI Vendor will be expecting to receive.  You see, we can accept a MSpeak3 message and transform this message
into a MSPeak4 Message.

Host URL: The endpoint for the AMI system. Something like :  http://demo.turtletech.com/latest/webAPI/MR_CB.asmx  

User ID:  The user the AMI system expects.  The AMI Vendor will need to give you a username. Ask Them for a Username.

User Password:  The password the AMI system expects. The AMI Vendor will need to give you a password. Ask them for a Password.

AMI Meter Type:
This is one is crucial. Very Very Special.

Each AMI system can accept generally one device id, and our MDM stores 3 device ids (MeterIdentifier, (Serial) AMISerialNo or AMIControlIdentifier).

Normal users our CSR Portal will typical just remember and use MeterIdentifier.  But sometimes the AMI systems require a different device id to issue
a command.  When the AMI requires an id different than MeterIdentifier, you can either choose Serial, or AMIControlIdenifier.  The AMI vendor
will tell you which id they will expect.  Ask them. Ask someone.  Test. Test Again.  And Test your Test.

Step Final: Test that thing...!





## Outage Management Systems

Typically we will receive InitiateOutageDetectionEventRequest from OMS.  This request is
routed to the correct AMI and the AMI responds with an ODEventNotification.

## Information Needed before any setup is completed

CIS Systems will be sending requests to the MultiSpeak Broker.
The CIS system will need an endpoint to send data to, which ElectSolve will setup
and give the CIS Vendor the following:

Company Name, they have in the MultiSpeak Header
User
Pwd

## Step 1 Authorized User

Add Server for the Clients User

Client - MDMPortal / CSR Portal
Server - MultiSpeakBroker

Add Authorized User for the MultiSpeak Broker Web App.

### Example 1

Example - CSR Portal Makes a call to the MultiSpeak Broker
Authorized Users - http://10.86.1.81/MultiSpeakBroker/UserConfig.aspx

Company : BackOffice
UserName : bo
User Password: bo

This company, user and password will be used to post data to the MultiSpeakBroker Url:

> https://etss-apploadtest.etss.com/MultiSpeakBroker/MultiSpeak/416/1/MDM_Server.asmx

db.User.find({Compnay:"BackOffice"}).pretty()
{
        "_id" : ObjectId("58ed494497c3742734702331"),
        "Company" : "BackOffice",
        "UserID" : "bo",
        "Password" : "bo"
}

#### Automate

```mongo

db.User.insert()
{
        "Company" : "BackOffice",
        "UserID" : "bo",
        "Password" : "bo"
}

```

### Example 2

Example - CIS Vendor MultiSpeak Broker
Authorized Users - http://10.86.1.81/MultiSpeakBroker/UserConfig.aspx

Company : NISC
UserName : NISC
User Password: 12345

This company, user and password will be used to post data to the MultiSpeakBroker Url:

> https://etss-apploadtest.etss.com/MultiSpeakBroker/MultiSpeak/416/1/MDM_Server.asmx

``` mongo

db.User.find({Compnay:"BackOffice"}).pretty()
{
        "_id" : ObjectId("58ed494497c3742734702331"),
        "Company" : "NISC",
        "UserID" : "NISC",
        "Password" : "12345sbo
}

```

#### Automate

```mongo

db.User.insert()
{
        "Company" : "BackOffice",
        "UserID" : "bo",
        "Password" : "bo"
}

```

This will allow the CSR portal to send requests to the MultiSpeakBroker Web App.
For example, in the MDMPortal.config you would setup the following:

J:\ElectSolve\Green\Config\MDMPortal.config

```xml

  <add key="Multispeak.415.Generic.User" value="bo" />
  <add key="Multispeak.415.Generic.Password" value="bo" />
  <add key="Multispeak.415.Generic.ApplicationName" value="MDM" />
  <add key="Multispeak.415.Generic.Company" value="BackOffice" />
  <add key="Multispeak.415.Generic.ApplicationVersion" value="2.0" />
  <add key="Multispeak.415.Generic.BaseURL" value="https://etss-apploadtest.etss.com/MultiSpeakBroker/MultiSpeak/416/1/" />
  <add key="Multispeak.415.Generic.CD_ServerPath" value="MDM_Server.asmx" />
  <add key="Multispeak.415.Generic.MR_ServerPath" value="MDM_Server.asmx" />
  <add key="Multispeak.415.Generic.Response.BaseURL" value="https://etss-apploadtest.etss.com/MDMServices/Multispeak/415/" />
  <add key="Multispeak.415.Generic.Response.NOT_ServerPath" value="NOT_Server.asmx" />
  <add key="Multispeak.415.Generic.Response.ConnectDisconnectList" value="N" />
  <add key="Multispeak.415.Generic.meterID.IsAttribute" value="N" />
  <add key="Multispeak.415.Generic.Response.ConvertWhToKwh" value="N" />
  <add key="Multispeak.415.Generic.Response.NotServerReadMethod" value="ReadingChangedNotification" />

```

So what we have is a person using the page InitiateMeterRead, InitiateConnect, InitiateDisconnect, InitiateDemandReset
will forward or post this command to the MultiSpeak Broker.

It will be the MultiSpeak Broker's job to forward the request to the actual vendor (AMI, CIS, OMS, etc..)

## Step 2 - Vendor Configuration - http://10.86.1.81/MultiSpeakBroker/VendorConfig.aspx

Step 2 has two parts.   The first part is adding the Authorized user we just setup which is going to receive
the requests from the CSR Portal to be able to choose the correct endpoint to send the requests.  We can
support many AMI systems, and each system has a ReadSource.  We use this read source to forward meter
commands to the correct AMI system.

As the broker receives requests, having the CSR Portal's Receiver (BackOffice) setup as a vendor we
can then setup this receiver to use the MDMWebAPI to query the MDM system about the meter in the request.
Each InitiateMeterRead, Connect, Disconnect, and Demand Reset will be for a meter, and a meter could be
in either Aclara or SRFN or Sensus, or SSN AMI system.

We map the meter's readsource when we inititally load the data.

### Step 2.A - Setup the CSR Portal's Authorized User as a Vendor.

https://etss-apploadtest.etss.com/MultiSpeakBroker/VendorConfig.aspx
Add
    Name : BackOffice
    Info : TYPE =  Client. Store credential to Backoffice

If this vendor is making requests to Broker, please configure Client Web API where Broker 
can retreive meter information.

**Configure the MDMWebAPi**

If Broker is required to return or notify result to this vendor, the vendor might expect user/pass. 
Configure Credential.

~This is more for synchronous responses which need their own example~

> db.Vendor.find({VendorName: "BackOffice"}).pretty()

```json

{
        "_id" : ObjectId("58e417c297c37811841113fe"),
        "VendorName" : "BackOffice",
        "MultiSpeakVersion" : "3",
        "MRCB_UserID" : "member0",
        "MRCB_Password" : "member0",
        "CDCB_UserID" : "member0",
        "CDCB_Password" : "member0",
        "Info" : "TYPE =  Client. Store credential to Backoffice",
        "APIUrl" : "https://etss-apploadtest.etss.com/MDMWebAPI/",
        "APIUserID" : "electsolve",
        "APIPassword" : "3lects01ve!"
}

### Step 2.B - Setup the Vendor's endpoint who will recieve the request and return a response.

This step adds the Vendor who will receive the request.  The vendor could be an AMI, CIS, or OMS system.

Typically for Asynchronous request and responses, where we send a request, and move on, and then server then
responds with a Notification, we will ndver have to setup the MR_BC and CD_CB user and passwords.

The main area to pay attention to here is the VendorName which needs to match the 
MDM.dbo.ReadSource.ReadSourceDescription.

> db.Vendor.find({VendorName: "SSN"}).pretty()
{
        "_id" : ObjectId("5880ead597c37b1bc0905f2b"),
        "VendorName" : "SSN",
        "MultiSpeakVersion" : "4",
        "MRCB_UserID" : "",
        "MRCB_Password" : "",
        "CDCB_UserID" : "",
        "CDCB_Password" : "",
        "Info" : "TYPE = RS. Shared ReadSource (for all utilies)"
}

```

### Step 2.C

Click 'Set Subscribers'
> https://etss-apploadtest.etss.com/MultiSpeakBroker/SubscribersConfig.aspx?ReadSource=SSN

Once you have the Vendor setup (AMI, CIS, OMS) we have to configure or setup the requests we
will allow the MultiSpeakBroker to forward on to the AMI, CIS, or OMS System.

Each method will need to be setup individually.

Edit Subscriber - InitiateCDStateRequest (MSpeak4)
Response Method(MSpeak4): MSpeak4CDStateChangedNotification

Host: **SSN**  (Available from Dropdown) - This should match your vendor
Host URL:  http://10.86.1.32:8081/soapserver
User ID:
User Password:
AMI Meter Type:

AMI Meter Type is **VERY** important.  AMI systems typically can accept one value
which is used to look up the meter and forward the command to the meter.

Remember we have two types of messages we have to setup:

- Commands which go to the AMI
- Commands which are coming back from AMI as Notifications.

We will send a multispeak message version 4 message (InitiateCDStateRequest) to
the SSN AMI system.

Add/Edit Subscriber
Channel: ChannelMSpeak4InitiateCDStateRequest
Receiver Host Type: SSN 
Host URL:  http://10.86.1.32:8081/soapserver
User ID  ******
User Password  ****
AMI Meter Type  MeterIdntifier

The user and password you enter here will be used to authenticate with the AMI provider URLs
and API methods.

Examples

> db.Subscriber.find({MeterReadSource: "SSN"}).pretty()

```json

{
        "_id" : ObjectId("58e4166a97c37811841113f5"),
        "MeterReadSource" : "SSN",
        "Type" : "SSN",
        "Channel" : "ChannelMSpeak4InitiateCDStateRequest",
        "Url" : "http://10.86.1.32:8081/soapserver",
        "UserID" : "",
        "Password" : "",
        "AMIMeterType" : "MeterIdentifier",
        "SettingAppendSerialNo" : false,
        "SettingSourceSerialNo" : null
}
{
        "_id" : ObjectId("58e4166a97c37811841113f6"),
        "MeterReadSource" : "SSN",
        "Type" : "MSpeak4",
        "Channel" : "ChannelMSpeak4CDStateChangedNotification",
        "Url" : "",
        "UserID" : "",
        "Password" : "",
        "AMIMeterType" : "MeterIdentifier",
        "SettingAppendSerialNo" : false,
        "SettingSourceSerialNo" : null
}
{
        "_id" : ObjectId("58e4166a97c37811841113f7"),
        "MeterReadSource" : "SSN",
        "Type" : "MSpeak4",
        "Channel" : "ChannelMSpeak4CDStatesChangedNotification",
        "Url" : "",
        "UserID" : "",
        "Password" : "",
        "AMIMeterType" : "MeterIdentifier",
        "SettingAppendSerialNo" : false,
        "SettingSourceSerialNo" : null
}
{
        "_id" : ObjectId("58e4167897c37811841113f8"),
        "MeterReadSource" : "SSN",
        "Type" : "SSN",
        "Channel" : "ChannelMSpeak4InitiateConnectDisconnect",
        "Url" : "http://10.86.1.32:8081/soapserver",
        "UserID" : "",
        "Password" : "",
        "AMIMeterType" : "MeterIdentifier",
        "SettingAppendSerialNo" : false,
        "SettingSourceSerialNo" : null
}
{
        "_id" : ObjectId("58e4168397c37811841113f9"),
        "MeterReadSource" : "SSN",
        "Type" : "SSN",
        "Channel" : "ChannelMSpeak4InitiateMeterReadingsByMeterID",
        "Url" : "http://10.86.1.32:8081/soapserver",
        "UserID" : "",
        "Password" : "",
        "AMIMeterType" : "AMIControlIdentifier",
        "SettingAppendSerialNo" : false,
        "SettingSourceSerialNo" : null
}
{
        "_id" : ObjectId("58e4168397c37811841113fa"),
        "MeterReadSource" : "SSN",
        "Type" : "MSpeak4",
        "Channel" : "ChannelMSpeak4ReadingChangedNotification",
        "Url" : "",
        "UserID" : "",
        "Password" : "",
        "AMIMeterType" : "MeterIdentifier",
        "SettingAppendSerialNo" : false,
        "SettingSourceSerialNo" : null
}
{
        "_id" : ObjectId("58e4169097c37811841113fb"),
        "MeterReadSource" : "SSN",
        "Type" : "SSN",
        "Channel" : "ChannelMSpeak4InitiateDemandReset",
        "Url" : "http://10.86.1.32:8081/soapserver",
        "UserID" : "",
        "Password" : "",
        "AMIMeterType" : "MeterIdentifier",
        "SettingAppendSerialNo" : false,
        "SettingSourceSerialNo" : null
}
{
        "_id" : ObjectId("58e4169097c37811841113fc"),
        "MeterReadSource" : "SSN",
        "Type" : "MSpeak4",
        "Channel" : "ChannelMSpeak4MeterEventNotification",
        "Url" : "",
        "UserID" : "",
        "Password" : "",
        "AMIMeterType" : "MeterIdentifier",
        "SettingAppendSerialNo" : false,
        "SettingSourceSerialNo" : null
}

```

## Done

Are we done?

## InfoSec

Gathering knowledge about what is deployed - regarding the broker at a specific location.

Steps
- Get Location of mongo
- Get user and pwd of mongo
- If you don't have any of these in the wiki... you will have to view the MultiSpeakBroker Web App web.config or MultiSpeak Broker 
Service MultiSpeakBrokerService.exe.config.

