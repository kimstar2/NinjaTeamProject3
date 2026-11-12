using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/Change State")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "Change State", message: "Change State to [State]", category: "Events", id: "5b4032a2b151ab7e19eabee006d5babb")]
public sealed partial class ChangeState : EventChannel<EnemyState> { }

