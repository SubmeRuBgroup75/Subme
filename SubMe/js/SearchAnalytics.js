const subletDurationCategory = [3, 5, 10, 14, 21, 30, 90]; //The duration of sublet ads that the user (searcher) marked as a like

const maxPriceCategory = [1500, 3000, 4500, 6000, 7500, 9000, 10500, 12000]; //The maximum price a user searched for a sublet ad

const commonCountryAreaCategory = [0, 4, 2, 3, 7]; //In order to find the user's most common area search at israel (haifa, jerusalem,tlv, eilat)

// const commonCityAreaCategory = ["Without preference", "North", "Center", "South"]; //In order to find the user's most common area search at the city

var personPriceCategory;
var personCityCategory;
var personCityAreaCategory;
var personDurationCategory;
var UserFBID = JSON.parse(localStorage["UserFBID"]);

function SearchAnalytics() {

    SubletSearchProp = JSON.parse(localStorage["AlgoJason"]);
    let dutationSublet = 10;

    personPriceCategory = categoryNumbersIndex(parseInt(SubletSearchProp.MaxBudjet), maxPriceCategory);
    personDurationCategory = categoryNumbersIndex(dutationSublet, subletDurationCategory);
    personCityCategory = categoryCountryIndex(SubletSearchProp.City, commonCountryAreaCategory);
    // personCityAreaCategory = 

    UserPref = {
        UserId: UserFBID,
        DurationKod: personDurationCategory,
        PriceKod: personPriceCategory,
        CityKod: personCityCategory
    };

    //Post User pre' to DB
    ajaxCall("POST", "../api/UserPreferences", JSON.stringify(UserPref), successAddPref, errorAddPref);

}

function categoryNumbersIndex(userSelection, arrCategory) {
    for (let i = 0; i < arrCategory.length; i++) {
        if (userSelection <= arrCategory[i]) {
            return i;
        }
    }
    return 0;
}

function categoryCountryIndex(userSelection, arrCategory) {
    for (let i = 0; i < arrCategory.length; i++) {
        if (userSelection === arrCategory[i]) {
            return i;
        }
    }
    return 0;
}

function successAddPref(data) {
    // Go to SmartProfile.js --> build Search Vector for user 
    alert("Succsess Add");
    InitMe();
    // location.href = "TinderSearch.html";
}

function errorAddPref(err) {
    console.log(err);
}

//------------------------------  SMART PROFILE JS  -------------------------------------

var userId = UserFBID, duration, durationBelonging, durationDeviationValue, price, priceBelonging, priceDeviationValue, absDeviation;

var WithoutPreferencePercent, HaifaPercent, JerusalemPercent, TlvPercent, EilatPercent;

const belongingVector = [
    {
        range: [0, 0.1], // range of deviation
        Percent: 0.9, // Percent of belonging
        name: "clearly Belonging "  // It means
    },
    {
        range: [0.11, 0.2],
        Percent: 0.8,
        name: "strong Belonging"
    },
    {
        range: [0.21, 0.25],
        Percent: 0.7,
        name: "Likely Belonging"
    },
    {
        range: [0.26, 0.3],
        Percent: 0.6,
        name: "medium Belonging"
    },
    {
        range: [0.31, 0.49],
        Percent: 0.4,
        name: "weak Belonging"
    }
];

//const subletDurationCategory = [3, 5, 10, 14, 21, 30, 90]; //The duration of sublet ads that the user (searcher) marked as a like

//const maxPriceCategory = [1500, 3000, 4500, 6000, 7500, 9000, 10500, 12000]; //The maximum price a user searched for a sublet ad

//const commonCountryAreaCategory = [0, 4, 2, 3, 7]; //In order to find the user's most common area search at israel (haifa, jerusalem,tlv, eilat)

//const commonCityAreaCategory = ["Without preference", "North", "Center", "South"]; //In order to find the user's most common area search at the city

function InitMe() {
    //GET previus search from DB
    ajaxCall("GET", "../api/UserPreferences/?uid=" + UserFBID, "", successGetAtt, errorGetAtt);
}

function successGetAtt(data) {
   // console.log(data);
    alert("Smart Profile Success");

    personDurationVector = buildDurationVector(data);
    duration = CalculateDurationVector(personDurationVector);
    durationBelonging = Belonging();

    personPriceArray = buildPriceVector(data);
    price = CalculateMaxPriceVector(personPriceArray);
    priceBelonging = Belonging(); 

    UserProfile = {
        UserId: UserFBID,
         DurationKod: parseInt(duration),
         DurationBelonging: durationBelonging , 
         DurationDeviationValue: durationDeviationValue , 
         PriceKod: parseInt(price),
         PriceBelonging: priceBelonging, 
         PriceDeviationValue: priceDeviationValue
    };

    ajaxCall("POST", "../api/UserProfile", JSON.stringify(UserProfile), successPostProfile, errorPostProfile);

    personCityArray = buildCityVector(data);
}

function successPostProfile(data) {    
    alert("Succsess create user profile");
    SmartCity(personCityArray);
}

function errorPostProfile(err) {
    alert("ERROR create user profile");
    console.log(err);
}


function errorGetAtt(err) {
    console.log(err);
}

function buildDurationVector(objList) {
    let arr = [];
    for (var i = 0; i < objList.length; i++) {
        arr.push(objList[i].DurationKod);  
    }

    return arr;
}

function buildPriceVector(objList) {
    let arr = [];
    for (var i = 0; i < objList.length; i++) {
        arr.push(objList[i].PriceKod);
    }

    return arr;
}

function buildCityVector(objList) {
    let arr = [];
    for (var i = 0; i < objList.length; i++) {
        arr.push(objList[i].CityKod);
    }

    return arr;
}

// A function that receives the vector of the searcher's DURATION SUBLET and calculates relevant values
function CalculateDurationVector(personDurationVector) {
    var sumOfTheVector = 0, meanResult, durationCategory, deviationValue = 0;

    for (let index = 0; index < personDurationVector.length; index++) {
        sumOfTheVector = sumOfTheVector + personDurationVector[index];
    }

    meanResult = sumOfTheVector / personDurationVector.length; // חישוב ממוצע לווקטור השיוך למשך חיפוש הסאבלט
    durationCategory = meanResult.toFixed(); // מעגל את התוצאה לשלם הקרוב ביותר
    absDeviation = Math.abs(meanResult - durationCategory); // הערך המוחלט של הסטייה 

    deviationValue = (meanResult - durationCategory) * subletDurationCategory[durationCategory]; // in order to know the number (money, duration...) of the deviation

    durationDeviationValue = deviationValue;

    return durationCategory;
}

// A function that receives the vector of the searcher's MAX PRICE SUBLET and calculates relevant values
function CalculateMaxPriceVector(personPriceArray) {
    var sumOfTheVector = 0, meanResult, priceCategory, deviationValue = 0;

    for (let index = 0; index < personPriceArray.length; index++) {
        sumOfTheVector = sumOfTheVector + personPriceArray[index];
    }

    meanResult = sumOfTheVector / personPriceArray.length;
    priceCategory = meanResult.toFixed();
    absDeviation = Math.abs(meanResult - priceCategory);

    deviationValue = (meanResult - priceCategory) * maxPriceCategory[priceCategory]; // in order to know the number (money, duration...) of the deviation

    priceDeviationValue = deviationValue;

    return priceCategory;
}

function Belonging() {
    let belonging = -1;
    for (let i = 0; i < belongingVector.length; i++) {
        if (absDeviation >= belongingVector[i].range[0] && absDeviation <= belongingVector[i].range[1]) {
            belonging = belongingVector[i].Percent;
            break;
        }
    }
    return belonging;
}

// A function that receives the vector of the searcher's MOST POPULAR CITY SUBLET and calculates relevant values
function SmartCity(personCityArray) {
   
    var city = [];
    city[0] = 0;
    city[4] = 0;
    city[2] = 0;
    city[3] = 0;
    city[7] = 0;

    var Percent00, Percent04, Percent02, Percent03, Percent07;

    for (let index = 0; index < personCityArray.length; index++) {

        city[personCityArray[index]]++;
    }

    /// parseFloat אור
    Percent00 = parseFloat(city[0] / personCityArray.length); // אחוז הפעמים שעיר מסויימת מופיעה מתוך סהכ הערים שחיפש המשתמש
    Percent04 = parseFloat(city[4] / personCityArray.length);
    Percent02 = parseFloat(city[2] / personCityArray.length);
    Percent03 = parseFloat(city[3] / personCityArray.length);
    Percent07 = parseFloat(city[7] / personCityArray.length);

    CityBelonging = {
        WithoutPreferencePercent : Percent00,
        HaifaPercent : Percent04,
        JerusalemPercent : Percent02,
        TlvPercent : Percent03,
        EilatPercent : Percent07
    };
    
    ajaxCall("POST", "../api/CityBelonging", JSON.stringify(CityBelonging), successPostCityBelonging, errorPostCityBelonging);
}

function successPostCityBelonging(data) {
    alert("Succsess post CityBelonging");
    location.href = "TinderSearch.html";
}

function errorPostCityBelonging(err) {
    console.log(err);
}