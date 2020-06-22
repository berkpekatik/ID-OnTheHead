using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace IdHead
{
    public class Main : BaseScript
    {
        private static List<object> IdList = new List<object>();
        public Main()
        {
            Tick += OnTick;
            Tick += OnTickId;
        }
        private async Task OnTickId()
        {
            if (!IsControlReleased(0, 19) && IdList.Count > 0)
            {
                foreach (var ids in IdList)
                {
                    var id = int.Parse(ids.ToString());
                    Draw3DText(GetEntityCoords(GetPlayerPed(id), true), GetPlayerServerId(id).ToString(), 255, 255, 255);
                }
            }
        }
        private async Task OnTick()
        {
            IdList = GetActivePlayers();
            await Delay(5000);
        }

        private void Draw3DText(Vector3 vector, string name, int r, int g, int b, float camDist = 6.5f)
        {
            var p = GetGameplayCamCoord();
            var distance = GetDistanceBetweenCoords(p.X, p.Y, p.Z, vector.X, vector.Y, vector.Z + 1, false);
            if (distance > camDist)
            {
                return;
            }
            var coords = GetEntityCoords(PlayerPedId(), false);
            if (Vdist2(coords.X, coords.Y, coords.Z, vector.X, vector.Y, vector.Z + 1) < 1000F)
            {
                float x = 0f, y = 0f;
                var onscreen = World3dToScreen2d(vector.X, vector.Y, vector.Z + 1, ref x, ref y);

                var scale = (1f / distance) * (2f);
                var fov = (1f / GetGameplayCamFov()) * 75f;
                scale = scale * fov;
                if (onscreen)
                {
                    
                    SetTextScale(0, scale);
                    SetTextFont(0);
                    SetTextProportional(true);
                    SetTextColour(r, g, b, 255);
                    SetTextDropshadow(0, 0, 0, 0, 255);
                    SetTextEdge(2, 0, 0, 0, 150);
                    SetTextOutline();
                    SetTextEntry("STRING");
                    SetTextCentre(true);
                    AddTextComponentString(name);
                    DrawText(x, y);
                }
            }
        }
    }
}
