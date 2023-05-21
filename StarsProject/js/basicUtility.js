
// ==================================================================================================================================
// Purpose : Basic jQuery functionality file
// ==================================================================================================================================

// Function to create Polyline

var boundaryColor = '#ED1B24'; // initialize color of polyline
var polyCoordinates =[]; // initialize an array where we store latitude and longitude pair
var count=0;

function createPolyline(polyC)
{
 Path = new google.maps.Polyline({ path: polyC, strokeColor: boundaryColor, strokeOpacity: 1.0, strokeWeight: 2 });
 Path.setMap(map);
}

function connectPoints() {
    var point_add = []; // initialize an array
    var start = polyCoordinates[0]; // storing start point
    var end = polyCoordinates[(polyCoordinates.length - 1)]; // storing end point
    
    // pushing start and end point to an array
    point_add.push(start);
    point_add.push(end);
    createPolyline(point_add); // function to join points
    return polyCoordinates;
}