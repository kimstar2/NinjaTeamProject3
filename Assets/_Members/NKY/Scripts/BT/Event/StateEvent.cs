using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/StateEvent")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "StateEvent", message: "Change State To [State]", category: "Events", id: "d040c0a86380a60005b0647cf316da0c")]
public sealed partial class StateEvent : EventChannel<EnemyState> { }

