var userId = 123, duration, durationBelonging, durationDeviationValue, price, priceBelonging, priceDeviationValue, absDeviation;

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

const subletDurationCategory = [3, 5, 10, 14, 21, 30, 90]; //The duration of sublet ads that the user (searcher) marked as a like

const maxPriceCategory = [1500, 3000, 4500, 6000, 7500, 9000, 10500, 12000]; //The maximum price a user searched for a sublet ad

const commonCountryAreaCategory = [0, 4, 2, 3, 7]; //In order to find the user's most common area search at israel (haifa, jerusalem,tlv, eilat)

//const commonCityAreaCategory = ["Without preference", "North", "Center", "South"]; //In order to find the user's most common area search at the city

function InitMe() {
    //GET previus search from DB
    ajaxCall("GET", str, "", successGetAtt, errorGetAtt);
}

function successGetAtt(data) {
    console.log(data);
    alert("Smart Profile Success");
    //duration = CalculateDurationVector(data);
    //price = CalculateMaxPriceVector(personPriceArray);
    //durationBelonging = Belonging();
    //priceBelonging = Belonging();
    //SmartCity(personCityArray);
}

function errorGetAtt(err) {
    console.log(err);
}

// A function that receives the vector of the searcher's DURATION SUBLET and calculates relevant values
function CalculateDurationVector(personDurationVector) {
    var sumOfTheVector = 0, meanResult, durationCategory, deviationValue = 0, absDeviationCal;

    for (let index = 0; index < personDurationVector.length; index++) {
        sumOfTheVector = sumOfTheVector + personDurationVector[index];
    }

    meanResult = sumOfTheVector / personDurationVector.length; // חישוב ממוצע לווקטור השיוך למשך חיפוש הסאבלט
    durationCategory = meanResult.toFixed(); // מעגל את התוצאה לשלם הקרוב ביותר
    absDeviationCal = Math.abs(meanResult - durationCategory); // הערך המוחלט של הסטייה 

    //   absDeviation = absDeviationCal;

    deviationValue = (meanResult - durationCategory) * subletDurationCategory[durationCategory]; // in order to know the number (money, duration...) of the deviation

    durationDeviationValue = deviationValue;

    return durationCategory;
}

// A function that receives the vector of the searcher's MAX PRICE SUBLET and calculates relevant values
function CalculateMaxPriceVector(personPriceArray) {
    var sumOfTheVector = 0, meanResult, priceCategory, deviationValue = 0, absDeviationCal;

    for (let index = 0; index < personPriceArray.length; index++) {
        sumOfTheVector = sumOfTheVector + personPriceArray[index];
    }

    meanResult = sumOfTheVector / personPriceArray.length;
    priceCategory = meanResult.toFixed();
    absDeviation = Math.abs(meanResult - priceCategory);

    // absDeviation = absDeviationCal;

    deviationValue = (meanResult - priceCategory) * maxPriceCategory[priceCategory]; // in order to know the number (money, duration...) of the deviation

    priceDeviationValue = deviationValue;

    return priceCategory;
}

function Belonging() {
    let belonging;
    for (let i = 1; i <= belongingVector.length; i++) {
        if (absDeviation >= belongingVector[i].range[0] && absDeviation <= belongingVector[i].range[1]) {
            belonging = belongingVector[i].Percent;
            break;
        }
        return belonging;
    }
}

// A function that receives the vector of the searcher's MOST POPULAR CITY SUBLET and calculates relevant values
function SmartCity(personCityArray) {
    //  userId: this.props.userId,

    var city = [];
    city["0"] = 0;
    city["4"] = 0;
    city["2"] = 0;
    city["3"] = 0;
    city["7"] = 0;

    var Percent00, Percent04, Percent02, Percent03, Percent07;

    for (let index = 0; index < personCityArray.length; index++) {
        this.city[personCityArray[index]]++;
    }

    Percent00 = city["0"] / personCityArray.length; // אחוז הפעמים שעיר מסויימת מופיעה מתוך סהכ הערים שחיפש המשתמש
    Percent04 = city["4"] / personCityArray.length;
    Percent02 = city["2"] / personCityArray.length;
    Percent03 = city["3"] / personCityArray.length;
    Percent07 = city["7"] / personCityArray.length;


    WithoutPreferencePercent = Percent00;
    HaifaPercent = Percent04;
    JerusalemPercent = Percent02;
    TlvPercent = Percent03;
    EilatPercent = Percent07;

    // ajax to the cityareaController
}