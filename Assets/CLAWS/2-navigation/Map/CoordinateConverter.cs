using System;
using UnityEngine;

public class CoordinateConverter : MonoBehaviour
{
    private static readonly double K0 = 0.9996;
    private static readonly double E = 0.00669438;
    private static readonly double E2 = E * E;
    private static readonly double E3 = E * E * E;
    private static readonly double E_P2 = E / (1 - E);

    private static readonly double SQRT_E = Math.Sqrt(1 - E);
    private static readonly double _E = (1 - SQRT_E) / (1 + SQRT_E);
    private static readonly double _E2 = _E * _E;
    private static readonly double _E3 = _E * _E * _E;
    private static readonly double _E4 = _E * _E * _E * _E;
    private static readonly double _E5 = _E * _E * _E * _E * _E;

    private static readonly double M1 = 1 - E / 4 - 3 * E2 / 64 - 5 * E3 / 256;
    private static readonly double M2 = 3 * E / 8 + 3 * E2 / 32 + 45 * E3 / 1024;
    private static readonly double M3 = 15 * E2 / 256 + 45 * E3 / 1024;
    private static readonly double M4 = 35 * E3 / 3072;

    private static readonly double P2 = 3.0 / 2 * _E - 27.0 / 32 * _E3 + 269.0 / 512 * _E5;
    private static readonly double P3 = 21.0 / 16 * _E2 - 55.0 / 32 * _E4;
    private static readonly double P4 = 151.0 / 96 * _E3 - 417.0 / 128 * _E5;
    private static readonly double P5 = 1097.0 / 512 * _E4;

    private static readonly double R = 6378137;

    private static readonly string ZONE_LETTERS = "CDEFGHJKLMNPQRSTUVWXX";

    public static (double latitude, double longitude) ToLatLon(double easting, double northing, int zoneNum, char zoneLetter, bool? northern = null, bool strict = true)
    {
        if (northern == null && zoneLetter == '\0')
        {
            throw new ArgumentException("Either zoneLetter or northern needs to be set");
        }
        else if (northern != null && zoneLetter != '\0')
        {
            throw new ArgumentException("Set either zoneLetter or northern, but not both");
        }

        if (strict)
        {
            if (easting < 100000 || easting >= 1000000)
            {
                throw new ArgumentOutOfRangeException("Easting out of range (must be between 100 000 m and 999 999 m)");
            }
            if (northing < 0 || northing > 10000000)
            {
                throw new ArgumentOutOfRangeException("Northing out of range (must be between 0 m and 10 000 000 m)");
            }
        }
        if (zoneNum < 1 || zoneNum > 60)
        {
            throw new ArgumentOutOfRangeException("Zone number out of range (must be between 1 and 60)");
        }
        if (zoneLetter != '\0')
        {
            zoneLetter = char.ToUpper(zoneLetter);
            if (ZONE_LETTERS.IndexOf(zoneLetter) == -1)
            {
                throw new ArgumentOutOfRangeException("Zone letter out of range (must be between C and X)");
            }
            northern = zoneLetter >= 'N';
        }

        double x = easting - 500000;
        double y = northing;

        if (!northern.Value)
        {
            y -= 1e7;
        }

        double m = y / K0;
        double mu = m / (R * M1);

        double pRad = mu + P2 * Math.Sin(2 * mu) + P3 * Math.Sin(4 * mu) + P4 * Math.Sin(6 * mu) + P5 * Math.Sin(8 * mu);

        double pSin = Math.Sin(pRad);
        double pSin2 = pSin * pSin;

        double pCos = Math.Cos(pRad);

        double pTan = Math.Tan(pRad);
        double pTan2 = pTan * pTan;
        double pTan4 = pTan * pTan * pTan * pTan;

        double epSin = 1 - E * pSin2;
        double epSinSqrt = Math.Sqrt(epSin);

        double n = R / epSinSqrt;
        double r = (1 - E) / epSin;

        double c = _E * pCos * pCos;
        double c2 = c * c;

        double d = x / (n * K0);
        double d2 = d * d;
        double d3 = d * d * d;
        double d4 = d * d * d * d;
        double d5 = d * d * d * d * d;
        double d6 = d * d * d * d * d * d;

        double latitude = pRad - (pTan / r) * (d2 / 2 - d4 / 24 * (5 + 3 * pTan2 + 10 * c - 4 * c2 - 9 * E_P2) + d6 / 720 * (61 + 90 * pTan2 + 298 * c + 45 * pTan4 - 252 * E_P2 - 3 * c2));
        double longitude = (d - d3 / 6 * (1 + 2 * pTan2 + c) + d5 / 120 * (5 - 2 * c + 28 * pTan2 - 3 * c2 + 8 * E_P2 + 24 * pTan4)) / pCos;

        return (ToDegrees(latitude), ToDegrees(longitude) + ZoneNumberToCentralLongitude(zoneNum));
    }

    private static double ZoneNumberToCentralLongitude(int zoneNum)
    {
        return (zoneNum - 1) * 6 - 180 + 3;
    }

    private static double ToDegrees(double rad)
    {
        return rad / Math.PI * 180;
    }
}