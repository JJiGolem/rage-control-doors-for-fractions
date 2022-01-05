using System;
using System.Data;
using Newtonsoft.Json;
using GTANetworkAPI;
using GolemoSDK;

namespace Golemo.Fractions
{
    class DoorsControl : Script
    {
        [ServerEvent(Event.ResourceStart)]
        public void ResourceStart()
        {
            DataTable result = MySql.QueryRead("SELECT * FROM `fractionsdoors`");
            if (result == null || result.Rows.Count == 0) return;
            foreach (DataRow row in result.Rows)
            {
                int id = Convert.ToInt32(row["id"]);
                int fractionId = Convert.ToInt32(row["fractionId"]);
                int fractionRank = Convert.ToInt32(row["fractionRank"]);
                Vector3 RightPosition = JsonConvert.DeserializeObject<Vector3>(row["RightPosition"].ToString());
                Vector3 LeftPosition = JsonConvert.DeserializeObject<Vector3>(row["LeftPosition"].ToString());
                string model = Convert.ToString(row["Model"]);

                new FractionDoor(id, fractionId, fractionRank, model, RightPosition, LeftPosition);
            }
        }

        [Command("setrankdoor")]
        public static void ChangeAcessRankFracDoor(Player player, int rank)
        {
            if (!Main.Players.ContainsKey(player)) return;
            var acc = Main.Players[player];
            if (player.HasData("FRACTIONDOOR") 
                && Configs.FractionRanks[acc.FractionId].Count == acc.FractionLvl)
            {
                FractionDoor door = player.GetData<FractionDoor>("FRACTIONDOOR");
                door.ChangeAcessRank(rank);
            }
        }

        [RemoteEvent("fractionDoor::control")]
        public void FractionDoorControl(Player player)
        {
            if (!Main.Players.ContainsKey(player)) return;
            var acc = Main.Players[player];
            if (player.HasData("FRACTIONDOOR") && acc.FractionId > 0)
            {
                FractionDoor door = player.GetData<FractionDoor>("FRACTIONDOOR");
                door.ChangeState(acc.FractionId, acc.FractionLvl);
            }
        }
    }

    class FractionDoor
    {
        private int _id;
        private int _fractionId;
        private int _accessRank;

        private string _model;
        private Vector3 _rightDoorPosition;
        private Vector3 _leftDoorPosition;
        private Vector3 _center;

        private bool _locked = false;

        private ColShape _shape;

        public FractionDoor(int id, int fractionId, int accessRank, string model, Vector3 rightposition = null, Vector3 leftPosition = null)
        {
            _id = id;
            _fractionId = fractionId;
            _accessRank = accessRank;
            _model = model;
            _rightDoorPosition = rightposition;
            _leftDoorPosition = leftPosition;
            _center = rightposition;

            if (leftPosition != null)
                _center = (rightposition + leftPosition) / 2;

            CreateGTAElements();
        }

        private void CreateGTAElements()
        {
            float radius = 1;
            if (_center != _rightDoorPosition)
                radius = _center.DistanceTo2D(_rightDoorPosition);

            float diameter = radius;
            Vector3 pos = _center - new Vector3(0, 0, 0.5);
            _shape = NAPI.ColShape.CreateCylinderColShape(pos, diameter, diameter, 0);
            _shape.OnEntityEnterColShape += (s, e) =>
            {
                e.SetData("FRACTIONDOOR", this);
                Trigger.ClientEvent(e, "fractionDoors::enter_sync", _fractionId, _accessRank, _model, _rightDoorPosition, _leftDoorPosition, _locked);
            };
            _shape.OnEntityExitColShape += (s, e) =>
            {
                e.ResetData("FRACTIONDOOR");
                Trigger.ClientEvent(e, "fractionDoors::exit");
            };

            //NAPI.Marker.CreateMarker(1, pos, new Vector3(), new Vector3(), diameter * 2, new Color(255, 0, 0), false, 0);
        }

        public void ChangeAcessRank(int newRank)
        {
            if (newRank > 0 && newRank <= Configs.FractionRanks[_fractionId].Count)
            {
                _accessRank = newRank;
                MySql.Query($"UPDATE fractionsdoors SET fractionRank={newRank} WHERE id={_id}");
            }
        }
        public void ChangeState(int fractionId, int fractionRank)
        {
            if (_fractionId != fractionId || _accessRank > fractionRank)
                return;

            _locked = !_locked;
            Trigger.ClientEventInRange(_center, 5, "fractionDoors::sync", _model, _rightDoorPosition, _leftDoorPosition, _locked);
        }
    }
}
