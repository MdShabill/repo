CREATE TABLE SiteServiceProviders (
    Id INT PRIMARY KEY IDENTITY,
    SiteId INT NOT NULL,
    ServiceProviderId INT NOT NULL,
    ServiceTypeId INT NOT NULL,
    
    CONSTRAINT [FK_SiteServiceProviders_Sites] FOREIGN KEY (SiteId) REFERENCES Sites(Id),
    CONSTRAINT [FK_SiteServiceProviders_ServiceProviders] FOREIGN KEY (ServiceProviderId) REFERENCES ServiceProviders(Id),
    CONSTRAINT [FK_SiteServiceProviders_ServiceTypes] FOREIGN KEY (ServiceTypeId) REFERENCES ServiceTypes(Id)
);
