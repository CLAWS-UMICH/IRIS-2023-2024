using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Assumes the earth is a spheroid with a longer major axis than minor
// Will increase our accuracy of geodesic lengths but perhaps maybe more computationally expensive than last year
public class GPSUtils : MonoBehaviour
{
    static public Location originGPSCoords; // Origin coordinates of our player

    static double earthMajorAxisLengthInMeters = 6378137.0; // Semi-major axis length of the earth. This was the value used last year when assuming the earth was a sphere
    static double flatteningValue = 1 / 298.257223563; // Value describing the compression of a circle with semi major axis radius to form a ellipse

    static public void ChangeOriginGPSCoords(Location newOrigin)
    {
        originGPSCoords = newOrigin;
    }


    // Function used last year with the haversine formula, not as accurate but is more computationally efficient
    // Ran when Vincenty's algorithm doesn't converge in 1000 iterations.
    static private (double, double) DistanceAndAngleBetweenCoords(Location coords1, Location coords2)
    {
        //Debug.Log("Vincenty's failed, running Haversine");
        //Debug.Log(coords2);

        // Credit to https://www.movable-type.co.uk/scripts/latlong.html

        double theta_1 = coords1.latitude * Math.PI / 180;
        double theta_2 = coords2.latitude * Math.PI / 180;
        double delta_theta = (coords2.latitude - coords1.latitude) * Math.PI / 180;
        double delta_lambda = (coords2.longitude - coords1.longitude) * Math.PI / 180;

        double a = Math.Sin(delta_theta / 2) * Math.Sin(delta_theta / 2) +
                    Math.Cos(theta_1) * Math.Cos(theta_2) *
                    Math.Sin(delta_lambda / 2) * Math.Sin(delta_lambda / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        double d = earthMajorAxisLengthInMeters * c;

        double y = Math.Sin(delta_lambda) * Math.Cos(theta_2);
        double x = Math.Cos(theta_1) * Math.Sin(theta_2) -
                    Math.Sin(theta_1) * Math.Cos(theta_2) * Math.Cos(delta_lambda);
        double theta = Math.Atan2(y, x);

        double angle = theta * 180 / Math.PI;

        return (d, angle + 180);
    }

    static public Location AppPositionToGPSCoords(Vector3 appPosition)
    {
        double distanceFromOrigin = Math.Sqrt(Math.Pow(appPosition.x, 2) + Math.Pow(appPosition.z, 2));
        double angleFromOrigin = Math.Atan2(appPosition.z, appPosition.x) * 180 / Math.PI;

        double latitude = originGPSCoords.latitude * Math.PI / 180;  // Assuming originGPSCoords is known
        double longitude = originGPSCoords.longitude * Math.PI / 180;

        double delta = distanceFromOrigin / earthMajorAxisLengthInMeters;

        double newLatitude = Math.Asin(Math.Sin(latitude) * Math.Cos(delta) + Math.Cos(latitude) * Math.Sin(delta) * Math.Cos(angleFromOrigin * Math.PI / 180));
        double newLongitude = longitude + Math.Atan2(Math.Sin(angleFromOrigin * Math.PI / 180) * Math.Sin(delta) * Math.Cos(latitude),
                                                    Math.Cos(delta) - Math.Sin(latitude) * Math.Sin(newLatitude));

        newLatitude *= 180 / Math.PI;
        newLongitude *= 180 / Math.PI;

        return new Location(newLatitude, newLongitude);
    }

    static public Vector3 GPSCoordsToAppPosition(Location coords)
    {
        // (double distanceFromOrigin, double angleFromOrigin) = GPSCoordsAndAngleBetweenCoords(coords, originGPSCoords);
        (double distanceFromOrigin, double angleFromOrigin) = DistanceAndAngleBetweenCoords(coords, originGPSCoords);
        double distanceFromOriginX = distanceFromOrigin * Math.Cos(angleFromOrigin * Math.PI / 180);
        double distanceFromOriginZ = distanceFromOrigin * Math.Sin(angleFromOrigin * Math.PI / 180);

        return new Vector3((float)distanceFromOriginX, 0f, (float)distanceFromOriginZ);
    }
}