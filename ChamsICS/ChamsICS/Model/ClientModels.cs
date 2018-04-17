﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChamsICSWebService.Model
{
    public class CreateClientReq
    {
        public string Name { get; set; }
        public string Addres { get; set; }
        public string Email { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public int Status { get; set; }

        public AuditTrailData AuditTrailData { get; set; }
    }

    public class UpdateClientReq
    {
        public int ClientId { get; set; }
        public string ClientCode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public int Status { get; set; }

        public AuditTrailData AuditTrailData { get; set; }
    }

    public class CreateClientRes : Response
    {
        public string Code { get; set; }
        public string ClientId { get; set; }
    }

    public class FindClientRes : Response
    {
        public string ClientId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Addres { get; set; }
        public string Email { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public bool HasWebUsers { get; set; }
        public int status { get; set; }
        public ClientSetting ClientSetting { get; set; }
    }

    public class ClientSetting
    {
        public decimal? percentageDeduction { get; set; }
        public int? DefaultRevenueItemId { get; set; }
    }

    public class FindClientWithZoneRes : Response
    {
        public string ClientId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Addres { get; set; }
        public string Email { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public int status { get; set; }
        public bool HasWebUsers { get; set; }
        public ClientSetting ClientSetting { get; set; }
        public IEnumerable<Model.Agent> Agents { get; set; }
    }

    public class Client 
    {
        public string ClientId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Addres { get; set; }
        public string Email { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public int status { get; set; }
        public bool HasWebUsers { get; set; }
        public ClientSetting ClientSetting { get; set; }
        public AuditTrailData AuditTrailData { get; set; }
    }

    public class ClientWithZone
    {
        public string ClientId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Addres { get; set; }
        public string Email { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public int status { get; set; }
        public bool HasWebUsers { get; set; }
        public ClientSetting ClientSetting { get; set; }
        public AuditTrailData AuditTrailData { get; set; }
        public IEnumerable<Agent> Agents { get; set; }
    }

    public class GetAllClientsRes : Response
    {
        public IEnumerable<Client> Clients { get; set; }

    }

    public class GetAllClientsWithZoneRes : Response
    {
        public IEnumerable<ClientWithZone> Clients { get; set; }

    }


}
