﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fr8Data.Crates;
using Fr8Data.DataTransferObjects;
using Fr8Data.Manifests;
using Fr8Data.States;
using TerminalBase.Infrastructure;
using TerminalBase.Models;

namespace TerminalBase.Services
{
    public class UpstreamQueryManager
    {
        private readonly ActivityContext _activityContext;
        private readonly IHubCommunicator _hubCommunicator;
        private Guid ActivityId => _activityContext.ActivityPayload.Id;

        public UpstreamQueryManager(ActivityContext activityContext, IHubCommunicator hubCommunicator)
        {
            _activityContext = activityContext;
            _hubCommunicator = hubCommunicator;
        }

        public async Task<List<Crate<TManifest>>> GetCratesByDirection<TManifest>(CrateDirection direction)
        {
            return await _hubCommunicator.GetCratesByDirection<TManifest>(ActivityId, direction);
        }

        public async Task<List<TManifest>> GetCrateManifestsByDirection<TManifest>(CrateDirection direction)
        {
            return (await _hubCommunicator.GetCratesByDirection<TManifest>(ActivityId, direction)).Select(x => x.Content).ToList();
        }

        public async Task<List<Crate>> GetCratesByDirection(CrateDirection direction)
        {
            return await _hubCommunicator.GetCratesByDirection(ActivityId, direction);
        }

        public async Task<FieldDescriptionsCM> GetFieldDescriptions(CrateDirection direction, AvailabilityType availability)
        {
            return await _hubCommunicator.GetDesignTimeFieldsByDirection(ActivityId, direction, availability);
        }

        public async Task<Crate<FieldDescriptionsCM>> GetFieldDescriptionsCrate(string label, AvailabilityType availability)
        {
            var curUpstreamFields = await _hubCommunicator.GetDesignTimeFieldsByDirection(ActivityId, CrateDirection.Upstream, availability);
            return Crate<FieldDescriptionsCM>.FromContent(label, curUpstreamFields);
        }

        public async Task<List<CrateManifestType>> GetUpstreamManifestList()
        {
            var upstreamCrates = await GetCratesByDirection<Manifest>(CrateDirection.Upstream);
            return upstreamCrates/*.Where(x => !BaseTerminalActivity.ExcludedManifestTypes.Contains(x.ManifestType))*/.Select(f => f.ManifestType).Distinct().ToList();
        }

        public async Task<Crate<FieldDescriptionsCM>> GetUpstreamManifestListCrate(string label = "AvailableUpstreamManifests")
        {
            var manifestList = await GetUpstreamManifestList();
            var fields = manifestList.Select(f => new FieldDTO(f.Type, f.Id.ToString())).ToArray();

            return Crate<FieldDescriptionsCM>.FromContent(label, new FieldDescriptionsCM(fields));
        }

        public async Task<List<string>> GetUpstreamCrateLabelList()
        {
            var curCrates = await this.GetCratesByDirection<Manifest>(CrateDirection.Upstream);
            return curCrates/*.Where(x => !BaseTerminalActivity.ExcludedManifestTypes.Contains(x.ManifestType))*/.Select(f => f.Label).Distinct().ToList();
        }
        
        public async Task<Crate<FieldDescriptionsCM>> GetUpstreamCrateLabelListCrate(string label = "AvailableUpstreamLabels")
        {
            var labelList = await GetUpstreamCrateLabelList();
            var fields = labelList.Select(f => new FieldDTO(f, f)).ToArray();

            return Crate<FieldDescriptionsCM>.FromContent(label, new FieldDescriptionsCM (fields));
        }
    }
}
