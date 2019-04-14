var Cards;
var CardsCounter;
var CardsCounter1;
var index = 0;
var LikedSublets = [];
var UNLikedSublets = [];
var Locations = [];
var Info = [];
var SubletResults = [];
var SubLoc = [];
var IdString = "";
var SubImg = [];
var first;

$(document).ready(function () {

    //Popup Window for more sublet info
    //appends an "active" class to .popup and .popup-content when the "Open" button is clicked
    $(".open").on("click", function () {
        $(".popup-overlay, .popup-content").addClass("active");
    });

    //removes the "active" class to .popup and .popup-content when the "Close" button is clicked 
    $(".close, .popup-overlay").on("click", function () {
        $(".popup-overlay, .popup-content").removeClass("active");
    });

    //Does user has prior search?
    if (localStorage.getItem("SearchStr") === null) {
        //Send the user to search page
        let FirstRow = "You Haven't Defined Any Search Yet!";
        let SecondRow = "<br><a href='SubletSearch.html'>Click Here To Define Your Search</a>";
        let str = FirstRow + SecondRow;
        $('#card').html(str);
        return;
    }
    else {
        var SearchStr = localStorage["SearchStr"];
        //Ajaxcall to bring sublets details
        ajaxCall("GET", SearchStr, "", SuccessFIND, errorFIND);
    }

    //Get all Liked Sublets
    if (localStorage.getItem("LikedSublets") === null) {
        return;
    }
    else {
        let x = JSON.parse(localStorage["LikedSublets"]);
        for (var i = 0; i < x.length; i++) {
            LikedSublets[i] = {
                SubletID: x[i].SubletID,
                UserFBID: x[i].UserFBID
            };
        }
    }

});

function SuccessFIND(data) {
    first = true;
    CardsCounter1 = data.length;

    if (CardsCounter1 === 0) {
        let str = "No Results Found!";
        $('#card').html(str);
        return;
    }

    Cards = data;

    IdString = "";
        for (var i = 0; i < data.length; i++) {
            IdString += "SubletID = " + data[i].SubletID;
            if ( (i + 1) !== data.length) {
                IdString += " OR ";
            }
    }

    //Search Sublets WITH user defined properties
    let PropQuery = localStorage["QueryNiceToHave"];
    let PropStr = "../api/sublet/?SubletQuery=" + IdString + "&PropQuery=" + PropQuery;
    ajaxCall("GET", PropStr, "", SuccessFINDWithProp, errorFIND);

}

function SuccessFINDWithProp(data) {
    console.log(data);
    CardsCounter = data.length;

    IdString = "";
    for (var i = 0; i < data.length; i++) {
        IdString += "SubletID = " + data[i].SubletID;
        if ((i + 1) !== data.length) {
            IdString += " OR ";
        }
    }

    //get locations from DB - Save the string to use in Google Map
    localStorage["SubLocaltionAfterSearch"] = IdString;
    ajaxCall("GET", "../api/Location/?IdString=" + IdString, "", successGetLocation, errorGet);
}

function successGetLocation(data) {
    console.log(data);
    for (var i = 0; i < data.length; i++) {
        Locations[i] = {
            lat: parseFloat(data[i].Lat),
            lng: parseFloat(data[i].Lng)
        };
        Info[i] = {
            Street: data[i].Rout,
            ID: data[i].SubletID
        };
    }
    //GET SUBLET IMAGES
    ajaxCall("GET", "../api/SubletImage/?SubId=" + IdString, "", successGetImages, errorGet);
}

function successGetImages(data) {
    console.log(data);
    SubImg = data;
    CardStack();
}

function errorGet(err) {
    alert('EROR: ' + err);
}

function errorFIND(err) {
    console.log(err);
    alert("EROR: " + err);
}

function CardStack() {
    if (first) {
        alert("We Found " + CardsCounter + " Sublets for You!");
        var x = CardsCounter1 - CardsCounter;
        alert("If you search without properties you can find " + x + " more sublets!");
        first = false;
    }

    if (CardsCounter === index) {
        let FirstRow = "You Looked Throw All Your Results";
        let SecondRow = "<br><a href='LikedSublets.html'>Click Here To View The Sublets You Liked</a>";
        let str = FirstRow + SecondRow;
        $('#card').html(str);
        return;
    }

    SubLoc = {
        lat: Locations[index].lat,
        lng: Locations[index].lng
    };

    //CREATE IMAGE SLIDER IN POPUP
    //START LOOP
    var RowImg1 = '<div class="slide">';
    var RowImg2 = '<img src="' + SubImg[index].ImagePath1 + '" /> </div>';

    var RowImg3 = '<div class="slide">';
    var RowImg4 = '<img src="' + SubImg[index].ImagePath2 + '" /> </div>';

    var RowImg5 = '<div class="slide">';
    var RowImg6 = '<img src="' + SubImg[index].ImagePath3 + '" /> </div>';

    var RowImg7 = '<div class="slide">';
    var RowImg8 = '<img src="' + SubImg[index].ImagePath4 + '" /> </div>';

    var RowImg9 = '<div class="slide">';
    var RowImg10 = '<img src="' + SubImg[index].ImagePath5 + '" /> </div>';
    //END FOR LOOP

    let EndImg1 = '<div class="slideback"><</div>';
    let EndImg2 = '<div class="slidenext">></div>';
    let EndImg3 = '<div class="slidelist"> </div> </div> </div>';
    let ImgStr = RowImg1 + RowImg2 + RowImg3 + RowImg4 + RowImg5 + RowImg6 + RowImg7 + RowImg8 + RowImg9 + RowImg10 + EndImg1 + EndImg2 + EndImg3;
    $('.fadeslider').html(ImgStr);

    //Create Sublet Tinder Card
    let leftAr = '<img style="position:relative; float:left; top:120px" src="../img/left.png" />';
    let img = '<img src="' + SubImg[index].ImagePath1 + '" />';
    let rightAr = '<img style="position:relative; float:right; top:-120px" src="../img/right.png" />';
    let address = '<h3>' + Info[index].Street + '</h3> ';
    let row1 = '<p>Price: ' + Cards[index].Price + ' Check In: ' + Cards[index].CheckIn + ' Check Out: ' + Cards[index].CheckOut;
    let row2 = '</br>Floor: ' + Cards[index].FloorNo + ' Sqm: ' + Cards[index].SqMtr + ' Rooms: ' + Cards[index].NomOfRooms + '</p>';
    let row3 = '<p>' + Cards[index].Description + '</p>';
    let Mapbutton = '<button class="open" id="info"> <img src = "../img/map.png" /> </button> </br>';
    let DisLikebutton = '<button id="dislike" onclick="Dislike(' + Cards[index].SubletID + ')"> <img src = "../img/dislike_button.png" /> </button>';
    let Likebutton = '<button id="like" onclick="Like(' + Cards[index++].SubletID + ')"> <img src="../img/like_button.png" /> </button>';

    let str = img + address + row1 + row2 + row3 + DisLikebutton + Mapbutton + Likebutton;
    $('#card').html(str);
}

function successSublets(data) {
    console.log(data);
}

function erorSublets(err) {
    console.log(err);
    alert("EROR: " + err);
}

function Like(SubId) {
    var UID = localStorage["UserFBID"];

    for (var i = 0; i < LikedSublets.length; i++) {
        LikedSublets[i].UserFBID = UID;
        if (LikedSublets[i].SubletID === SubId) {
            alert("You Already Liked This Sublet");
            CardStack();
            return;
        }
    }
    LikedSublets[LikedSublets.length] = {
        SubletID: SubId,
        UserFBID: UID
    };
    console.log(LikedSublets);
    ajaxCall("POST", "../api/LikedSublets", JSON.stringify(LikedSublets), successInsrtFev, errorInsrtFev);
}

function successInsrtFev(data) {
    console.log("Sublet Favorite List Updated Succsessfuly!");
    CardStack();
}

function errorInsrtFev(err) {
    console.log(err);
}

function Dislike(SubId) {
    for (var i = 0; i < UNLikedSublets.length; i++) {
        if (UNLikedSublets[i] === SubId) {
            alert("You Already UnLiked This Sublet");
            CardStack();
            return;
        }
    }
    UNLikedSublets.push(SubId);
    console.log(UNLikedSublets.length);
    CardStack();
}

// Initialize and add the map
function initMap() {
    // The location of Uluru
    //var SubPos = SubLoc;
    // The map, centered at Uluru
    var map = new google.maps.Map(
        document.getElementById('map'), { zoom: 15, center: SubLoc });
    // The marker, positioned at Uluru
    var marker = new google.maps.Marker({ position: SubLoc, map: map });
}


/// Image Slider

$(function () {
    var i = 1;
    for (var b = 1; b <= $(".fadeslider .slide").length; b++) {
        $(".slidelist").append("<div></div>");
    }
    $(".slidelist div:nth-child(1)").addClass("active");
    function slider() {
        var l = $(".fadeslider .slide").length;
        if (i === l) { i = 0; }
        $(".fadeslider .slide").fadeOut(500);
        $(".slidelist div").removeClass("active");
        i++;
        $(".fadeslider .slide:nth-child(" + i + ")").fadeIn(500);
        $(".slidelist div:nth-child(" + i + ")").addClass("active");
        if (i >= l) { i = 0; }
    }

    function sliderback() {
        var l = $(".fadeslider .slide").length;
        if (i === 0) { i = l + 1; }
        $(".fadeslider .slide").fadeOut(500);
        $(".slidelist div").removeClass("active");
        i--;
        if (i <= 0) { i = l; }
        $(".fadeslider .slide:nth-child(" + i + ")").fadeIn(500);
        $(".slidelist div:nth-child(" + i + ")").addClass("active");
    }

    $(".slidenext").click(function () {
        slider();
    });
    $(".slideback").click(function () {
        sliderback();
    });
    var timer = setInterval(slider, 5000);
    $('.slideback,.slidenext').hover(function (ev) {
        clearInterval(timer);
    }, function (ev) {
        timer = setInterval(slider, 5000);
    });

});