using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Server.Services {
	public class SignalRGroupCollection {
		private readonly object lockObj = new();
		private readonly Dictionary<string, SignalRGroup> groups = new Dictionary<string, SignalRGroup>();
		private readonly Dictionary<string, HashSet<string>> connectionGroups = new Dictionary<string, HashSet<string>>();
		private readonly ILogger<SignalRGroupCollection> logger;

		public List<string> Groups => GetGroups();
		public List<string> Connections => GetConnections();

		public SignalRGroupCollection(ILogger<SignalRGroupCollection> logger) {
			this.logger = logger;
		}

		public List<string> GetGroups(string connection = null) {
			lock(lockObj) {
				if(connection == null)
					return new List<string>(groups.Keys);
				if(connectionGroups.TryGetValue(connection, out HashSet<string> connectionGroup))
					return new List<string>(connectionGroup);
				return new List<string>();
			}
		}

		public List<string> GetConnections(string groupName = null) {
			lock(lockObj) {
				if(groupName == null)
					return new List<string>(connectionGroups.Keys);
				if(groups.TryGetValue(groupName, out SignalRGroup group))
					return new List<string>(group.Connections);
				return new List<string>();
			}
		}

		public void AddOrUpdateGroup(string groupName, DateTime lastUpdate) {
			lock(lockObj) {
				if(!groups.TryGetValue(groupName, out SignalRGroup group)) {
					group = new SignalRGroup();
					groups.Add(groupName, group);
					logger.LogTrace($"Added group: {groupName}");
				}
				group.LastUpdate = lastUpdate;
			}
		}

		public void RemoveGroup(string groupName) {
			lock(lockObj) {
				if(groups.TryGetValue(groupName, out SignalRGroup group)) {
					foreach(string connection in new List<string>(group.Connections)) {
						RemoveConnectionFromGroupInternal(groupName, connection);
						TryRemoveConnectionInternal(connection);
					}
					groups.Remove(groupName);
					logger.LogTrace($"Removed group: {groupName}");
				}
			}
		}

		private void AddConnectionInternal(string connection) {
			if(!connectionGroups.TryGetValue(connection, out HashSet<string> connectionGroup)) {
				connectionGroup = new HashSet<string>();
				connectionGroups.Add(connection, connectionGroup);
				logger.LogTrace($"Connection {connection} added");
			}
		}

		public void AddConnectionToGroup(string groupName, string connection) {
			lock(lockObj) {
				if(!groups.ContainsKey(groupName))
					throw new Exception($"Group not found: {groupName}");
				AddConnectionInternal(connection);
				AddConnectionToGroupInternal(groupName, connection);
			}
		}

		private void AddConnectionToGroupInternal(string groupName, string connection) {
			bool existsConnectionGroup = connectionGroups.TryGetValue(connection, out HashSet<string> connectionGroup);
			if(existsConnectionGroup)
				connectionGroup.Add(groupName);
			bool existsGroupConnection = groups.TryGetValue(groupName, out SignalRGroup group);
			if(existsGroupConnection)
				group.Connections.Add(connection);
			if(existsConnectionGroup || existsGroupConnection)
				logger.LogTrace($"Connection {connection} added to group {groupName}");
			if(existsConnectionGroup != existsGroupConnection)
				logger.LogWarning($"Connection {connection} has inconsistent state for group {groupName}");
		}

		private void TryRemoveConnectionInternal(string connection) {
			if(connectionGroups.TryGetValue(connection, out HashSet<string> connectionGroup) && connectionGroup.Count == 0) {
				connectionGroups.Remove(connection);
				logger.LogTrace($"Connection {connection} removed");
			}
		}

		public void RemoveConnectionFromAllGroups(string connection) {
			lock(lockObj) {
				foreach(string name in new List<string>(groups.Keys))
					RemoveConnectionFromGroupInternal(name, connection);
				TryRemoveConnectionInternal(connection);
			}
		}

		private void RemoveConnectionFromGroupInternal(string groupName, string connection) {
			bool removed = false;
			if(groups.TryGetValue(groupName, out SignalRGroup group))
				removed |= group.Connections.Remove(connection);
			if(connectionGroups.TryGetValue(connection, out HashSet<string> connectionGroup))
				removed |= connectionGroup.Remove(groupName);
			if(removed)
				logger.LogTrace($"Connection {connection} removed from group {groupName}");
		}

		public List<string> SwapConnectionGroups(string groupNameToRemove, string groupNameToAdd) {
			lock(lockObj) {
				List<string> connections = new List<string>();
				if(!groups.ContainsKey(groupNameToAdd))
					throw new Exception($"Group not found: {groupNameToAdd}");
				if(groups.TryGetValue(groupNameToRemove, out SignalRGroup group)) {
					foreach(string connection in group.Connections) {
						RemoveConnectionFromGroupInternal(groupNameToRemove, connection);
						AddConnectionToGroupInternal(groupNameToAdd, connection);
						connections.Add(connection);
					}
				}
				return connections;
			}
		}

		private class SignalRGroup {
			public DateTime LastUpdate { get; set; }
			public HashSet<string> Connections { get; } = new HashSet<string>();
		}
	}
}
