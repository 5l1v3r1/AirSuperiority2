﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using GTA;
using GTA.Native;
using GTA.Math;

namespace AirSuperiority.ScriptBase.Helpers
{
    public static class Utility
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        public static bool IsForeground()
        {
            return System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle == GetForegroundWindow();
        }

        public static float GetRandomBetween(float a, float b)
        {
            return a > b ? Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, b, a) :
                Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, a, b);
        }

        /// <summary>
        /// Get a random positon in the area.
        /// </summary>
        /// <param name="toggle"></param>
        public static Vector3 GetRandomPositionInArea(Vector3 min, Vector3 max)
        {
            return new Vector3(GetRandomBetween(min.X, max.X),
                GetRandomBetween(min.Y, max.Y), 
                GetRandomBetween(min.Z, max.Z));
        }

        public static bool IsBetween(this float value, float min, float max)
        {
            return min > max ? (value < min && value > max) : (value > min && value < max);
        }

        public static double ToRadians(this float val)
        {
            return (Math.PI / 180) * val;
        }

        public static float HeadingTo(this Vector3 start, Vector3 end)
        {
            return Function.Call<float>(Hash.GET_HEADING_FROM_VECTOR_2D, end.X - start.X, end.Y - start.Y);
        }

        public static float DistanceTo(this float f, float value)
        {
            return Math.Abs(f - value);
        }

        public static Vector3 RotationToDirection(Vector3 Rotation)
        {
            double rotZ = Rotation.Z.ToRadians();
            double rotX = Rotation.X.ToRadians();
            double multiXY = Math.Abs(Convert.ToDouble(Math.Cos(rotX)));
            Vector3 res = default(Vector3);
            res.X = (float)(Convert.ToDouble(-Math.Sin(rotZ)) * multiXY);
            res.Y = (float)(Convert.ToDouble(Math.Cos(rotZ)) * multiXY);
            res.Z = (float)(Convert.ToDouble(Math.Sin(rotX)));
            return res;
        }

        /// <summary>
        /// Is the position within the provided bounds?
        /// </summary>
        /// <param name="toggle"></param>
        public static bool IsPositionInArea(Vector3 pos, Vector3 min, Vector3 max)
        {
            return IsBetween(pos.X, min.X, max.X) && 
                IsBetween(pos.Y, min.Y, max.Y) && 
                IsBetween(pos.Z, min.Z, max.Z);
        }

        public static void RequestPTFXAsset(string name)
        {
            if (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, name))
            {
                Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, name);
            }
        }

        /// <summary>
        /// Toggle online DLC maps.
        /// </summary>
        /// <param name="toggle"></param>
        public static void ToggleOnlineDLC(bool toggle)
        {
            Function.Call((Hash)0x0888C3502DBBEEF5, toggle);
        }

        /// <summary>
        /// Returns a 3D coordinate on a circle on the given point with the specified center, radius and total amount of points
        /// </summary>
        /// <param name="center">Center of the circle</param>
        /// <param name="radius">Total radius of the circle</param>
        /// <param name="totalPoints">Total points around circumference</param>
        /// <param name="currentPoint">The point on the circle for which to return a coordinate</param>
        /// <returns></returns>
        public static Vector3 Radiate(this Vector3 center, float radius, float totalPoints, float currentPoint)
        {
            float ptRatio = currentPoint / totalPoints;
            float pointX = center.X + (float)(Math.Cos(ptRatio * 2 * Math.PI)) * radius;
            float pointY = center.Y + (float)(Math.Sin(ptRatio * 2 * Math.PI)) * radius;
            Vector3 panelCenter = new Vector3(pointX, pointY, center.Z);
            return panelCenter;
        }

        public static void Teleport(this Player player, Vector3 p, float heading)
        {
            Function.Call(Hash.START_PLAYER_TELEPORT, player.Handle, p.X, p.Y, p.Z, heading, 0, 1, 1);

    /*        DateTime timeout = DateTime.Now + TimeSpan.FromMilliseconds(2000);

            while (!Function.Call<bool>((Hash)0xE23D5873C2394C61, player.Handle))
            {
                if (DateTime.Now > timeout) break;
            }

            Function.Call(Hash.STOP_PLAYER_TELEPORT);*/
        }


        /// <summary>
        /// Fade out screen (Since this function was removed with newer versions of Scripthookvdotnet)
        /// </summary>
        /// <param name="wait">The time to sleep while fading.</param>
        /// <param name="duration">The duration of the fade effect.</param>
        public static void FadeScreenOut(int duration)
        {
            Function.Call(Hash.DO_SCREEN_FADE_OUT, duration);
        }

        /// <summary>
        /// Fade in screen (Since this function was removed with newer versions of Scripthookvdotnet)
        /// </summary>
        /// <param name="wait">The time to sleep while fading.</param>
        /// <param name="duration">The duration of the fade effect.</param>
        public static void FadeScreenIn(int duration)
        {
            Function.Call(Hash.DO_SCREEN_FADE_IN, duration);
        }

        /// <summary>
        /// Load an item placement list by name.
        /// </summary>
        /// <param name="iplName">The IPL name.</param>
        public static void LoadItemPlacement(string iplName)
        {
            Function.Call(Hash.REQUEST_IPL, iplName);
        }

        /// <summary>
        /// Load multiple item placement lists from a list
        /// </summary>
        /// <param name="iplNames">The IPL names.</param>
        public static void LoadItemPlacements(IEnumerable<string> iplNames)
        {
            foreach (string ipl in iplNames)
            {
                LoadItemPlacement(ipl);
            }
        }

        /// <summary>
        /// Remove an item placement list by name.
        /// </summary>
        /// <param name="iplName">The IPL name.</param>
        public static void RemoveItemPlacement(string iplName)
        {
            if (Function.Call<bool>(Hash.IS_IPL_ACTIVE, iplName))
                Function.Call(Hash.REMOVE_IPL, iplName);
        }


        /// <summary>
        /// Remove multiple item placement lists from a list
        /// </summary>
        /// <param name="iplNames">The IPL names.</param>
        public static void RemoveItemPlacements(string[] iplNames)
        {
            foreach (string ipl in iplNames)
            {
                RemoveItemPlacement(ipl);
            }
        }

        /// <summary>
        /// Get a free location for spawning relative to the provided position.
        /// </summary>
        /// <returns></returns>
        public static Vector3 EnsureValidSpawnPos(Vector3 position)
        {
            for (int i = 0; i < 20; i++)
            {
                var pos = position.Around(20 * i);

                if (!Function.Call<bool>(Hash.IS_POSITION_OCCUPIED, pos.X, pos.Y, pos.Z, 10.0f, 0, 1, 0, 0, 0, 0, 0))
                {
                    return pos;
                }
            }

            return position;
        }
    }
}
