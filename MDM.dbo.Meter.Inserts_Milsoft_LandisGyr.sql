
-- Add your readsource which for meter centric operations
-- is your VendorName in the MultiSpeak Broker
USE [MDM]

INSERT INTO [dbo].[ReadSource]
           ([ReadSourceDescription]
           ,[ReadDatesStoredInUTC]
           ,[ReadSourceAMIURL]
           ,[CustomerPortalFlag]
           ,[Created]
           ,[ScadaLoadProfile]
           ,[AMILoadProfile]
           ,[UseMultiplier])
     VALUES
           ('Landis'
           ,0
           ,'Multispeak30z'
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,NULL)

-- Get the ReadSourceId to be used for all meter inserts
DECLARE @ReadSourceId INT
 SELECT @ReadSourceId = ReadSourceId
 FROM MDM.dbo.ReadSource
 
 -- Landis is 17 here

INSERT INTO [dbo].[Meter]
           ([MeterIdentifier]
           ,[MeterTypeId]
           ,[ManufacturerModelId]
           ,[LocationId]
           ,[AMISerialNo]
           ,[ManufacturerSerialNumber]
           ,[MeterStatusId]
           ,[MeterNote]
           ,[PurchaseDate]
           ,[ScrapDate]
           ,[ReadSourceId]
           ,[AMRMeterType]
           ,[Created]
           ,[LastUpdated]
           ,[SourceMeterIdentifier]
           ,[DisconnectCollarSerialNumber]
           ,[DisconnectCollarType]
           ,[RecordInterval]
           ,[Dials]
           ,[ServiceTypeId]
           ,[Multiplier]
           ,[FactoryIdNo]
           ,[IsBillable]
           ,[AMIControlIdentifier]
           ,[CurrentSwitchState]
           ,[Port])
     VALUES(100,1,175,42944,'fake',null,17,null,null,null,17,'iSA4',GETDATE(),GETDATE(),null,'NONE','NONE',null,null,1,1,null,null,null,null,null)


	 -- ServiceLocationChangedNotification requires a lookup so the meter needs this populated and valid
UPDATE MDM.dbo.Meter
	SET LocationId = 42944
	WHERE MeterIdentifier = '100'


UPDATE MDM.dbo.Meter
	SET LocationId = NULL
	WHERE MeterIdentifier = '100'
	 
INSERT INTO [dbo].[Meter]
           ([MeterIdentifier]
           ,[MeterTypeId]
           ,[ManufacturerModelId]
           ,[LocationId]
           ,[AMISerialNo]
           ,[ManufacturerSerialNumber]
           ,[MeterStatusId]
           ,[MeterNote]
           ,[PurchaseDate]
           ,[ScrapDate]
           ,[ReadSourceId]
           ,[AMRMeterType]
           ,[Created]
           ,[LastUpdated]
           ,[SourceMeterIdentifier]
           ,[DisconnectCollarSerialNumber]
           ,[DisconnectCollarType]
           ,[RecordInterval]
           ,[Dials]
           ,[ServiceTypeId]
           ,[Multiplier]
           ,[FactoryIdNo]
           ,[IsBillable]
           ,[AMIControlIdentifier]
           ,[CurrentSwitchState]
           ,[Port])
     VALUES('M2154890647',1,175,1,'fake',null,17,null,null,null,17,'iSA4',GETDATE(),GETDATE(),null,'NONE','NONE',null,null,1,1,null,null,null,null,null)

INSERT INTO [dbo].[Meter]
           ([MeterIdentifier]
           ,[MeterTypeId]
           ,[ManufacturerModelId]
           ,[LocationId]
           ,[AMISerialNo]
           ,[ManufacturerSerialNumber]
           ,[MeterStatusId]
           ,[MeterNote]
           ,[PurchaseDate]
           ,[ScrapDate]
           ,[ReadSourceId]
           ,[AMRMeterType]
           ,[Created]
           ,[LastUpdated]
           ,[SourceMeterIdentifier]
           ,[DisconnectCollarSerialNumber]
           ,[DisconnectCollarType]
           ,[RecordInterval]
           ,[Dials]
           ,[ServiceTypeId]
           ,[Multiplier]
           ,[FactoryIdNo]
           ,[IsBillable]
           ,[AMIControlIdentifier]
           ,[CurrentSwitchState]
           ,[Port])
     VALUES('M2154890648',1,175,1,'fake',null,17,null,null,null,17,'iSA4',GETDATE(),GETDATE(),null,'NONE','NONE',null,null,1,1,null,null,null,null,null)

INSERT INTO [dbo].[Meter]
           ([MeterIdentifier]
           ,[MeterTypeId]
           ,[ManufacturerModelId]
           ,[LocationId]
           ,[AMISerialNo]
           ,[ManufacturerSerialNumber]
           ,[MeterStatusId]
           ,[MeterNote]
           ,[PurchaseDate]
           ,[ScrapDate]
           ,[ReadSourceId]
           ,[AMRMeterType]
           ,[Created]
           ,[LastUpdated]
           ,[SourceMeterIdentifier]
           ,[DisconnectCollarSerialNumber]
           ,[DisconnectCollarType]
           ,[RecordInterval]
           ,[Dials]
           ,[ServiceTypeId]
           ,[Multiplier]
           ,[FactoryIdNo]
           ,[IsBillable]
           ,[AMIControlIdentifier]
           ,[CurrentSwitchState]
           ,[Port])
     VALUES('M2154890663',1,175,1,'fake',null,17,null,null,null,17,'iSA4',GETDATE(),GETDATE(),null,'NONE','NONE',null,null,1,1,null,null,null,null,null)

	 INSERT INTO [dbo].[Meter]
           ([MeterIdentifier]
           ,[MeterTypeId]
           ,[ManufacturerModelId]
           ,[LocationId]
           ,[AMISerialNo]
           ,[ManufacturerSerialNumber]
           ,[MeterStatusId]
           ,[MeterNote]
           ,[PurchaseDate]
           ,[ScrapDate]
           ,[ReadSourceId]
           ,[AMRMeterType]
           ,[Created]
           ,[LastUpdated]
           ,[SourceMeterIdentifier]
           ,[DisconnectCollarSerialNumber]
           ,[DisconnectCollarType]
           ,[RecordInterval]
           ,[Dials]
           ,[ServiceTypeId]
           ,[Multiplier]
           ,[FactoryIdNo]
           ,[IsBillable]
           ,[AMIControlIdentifier]
           ,[CurrentSwitchState]
           ,[Port])
     VALUES('D004018C',1,175,1,'fake',null,17,null,null,null,17,'iSA4',GETDATE(),GETDATE(),null,'NONE','NONE',null,null,1,1,null,null,null,null,null)

	INSERT INTO [dbo].[Meter]
           ([MeterIdentifier]
           ,[MeterTypeId]
           ,[ManufacturerModelId]
           ,[LocationId]
           ,[AMISerialNo]
           ,[ManufacturerSerialNumber]
           ,[MeterStatusId]
           ,[MeterNote]
           ,[PurchaseDate]
           ,[ScrapDate]
           ,[ReadSourceId]
           ,[AMRMeterType]
           ,[Created]
           ,[LastUpdated]
           ,[SourceMeterIdentifier]
           ,[DisconnectCollarSerialNumber]
           ,[DisconnectCollarType]
           ,[RecordInterval]
           ,[Dials]
           ,[ServiceTypeId]
           ,[Multiplier]
           ,[FactoryIdNo]
           ,[IsBillable]
           ,[AMIControlIdentifier]
           ,[CurrentSwitchState]
           ,[Port])
     VALUES('F07DF930',1,175,1,'fake',null,17,null,null,null,17,'iSA4',GETDATE(),GETDATE(),null,'NONE','NONE',null,null,1,1,null,null,null,null,null)
	
	
	INSERT INTO [dbo].[Meter]
           ([MeterIdentifier]
           ,[MeterTypeId]
           ,[ManufacturerModelId]
           ,[LocationId]
           ,[AMISerialNo]
           ,[ManufacturerSerialNumber]
           ,[MeterStatusId]
           ,[MeterNote]
           ,[PurchaseDate]
           ,[ScrapDate]
           ,[ReadSourceId]
           ,[AMRMeterType]
           ,[Created]
           ,[LastUpdated]
           ,[SourceMeterIdentifier]
           ,[DisconnectCollarSerialNumber]
           ,[DisconnectCollarType]
           ,[RecordInterval]
           ,[Dials]
           ,[ServiceTypeId]
           ,[Multiplier]
           ,[FactoryIdNo]
           ,[IsBillable]
           ,[AMIControlIdentifier]
           ,[CurrentSwitchState]
           ,[Port])
     VALUES('F0091875',1,175,1,'fake',null,17,null,null,null,17,'iSA4',GETDATE(),GETDATE(),null,'NONE','NONE',null,null,1,1,null,null,null,null,null)
