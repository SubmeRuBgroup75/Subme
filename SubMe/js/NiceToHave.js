function showDiv(divId, element) {
    document.getElementById(divId).className = element.value === 1 ? 'block' : 'none';
}

function GetProp() {

    var NiceToHaveArr = [];

    //NiceToHaveObject = {
    //    UserId: UserFBID,
    //    DurationKod: personDurationCategory,
    //    PriceKod: personPriceCategory,
    //    CityKod: personCityCategory
    //};


    let a1 = $("#Smoking").val();
    if (a1 === '1') {
        NiceToHaveArr.push("isBared");
    }

    let a2 = $("#balcony").val();
    if (a2 === '1') {

        NiceToHaveArr.push("isBalcony");
    }

    let a3 = $("#Accessible").val();
    if (a3 === '1') {

        NiceToHaveArr.push("isAccessible");
    }

    let a4 = $("#Renovated").val();
    if (a4 === '1') {

        NiceToHaveArr.push("isRenovated");
    }

    let a5 = $("#Elevator").val();
    if (a5 === '1') {

        NiceToHaveArr.push("isElevator");
    }

    let a6 = $("#Air-conditioned").val();
    if (a6 === '1') {

        NiceToHaveArr.push("isConditioner");
    }

    let a7 = $("#Storage").val();
    if (a7 === '1') {

        NiceToHaveArr.push("isStorage");
    }

    let a8 = $("#Pets-Friendly").val();
    if (a8 === '1') {

        NiceToHaveArr.push("isAnimals");
    }

    let a9 = $("#Yard").val();
    if (a9 === '1') {

        NiceToHaveArr.push("isYard");
    }

    let a10 = $("#Parking").val();
    if (a10 === '1') {

        NiceToHaveArr.push("isParking");
    }

    var q = "";

    for (var i = 0; i < NiceToHaveArr.length; i++) {
        q += NiceToHaveArr[i] + " = 1";
        if ((i + 1) !== NiceToHaveArr.length) {
            q += " AND ";
        }
    }


    localStorage["QueryNiceToHave"] = q;
}