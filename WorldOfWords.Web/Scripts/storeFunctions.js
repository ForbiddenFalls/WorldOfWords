function successfulBuy(data) {
    $("#word-" + data.wordId + "-quantity").text(data.newQuantity);
    $("#word-" + data.wordId + "-user-quantity").text(data.newUserQuantity);
    alert("You successfuly bought this word");
}

function failedBuy(error) {
    alert(error.statusText);
}

function addShopItemToList(shopItem, shopList) {
    var indexOfItemInArray = -1;
    for (var i = 0; i < shopList.length; i++) {
        var item = shopList[i];
        if (shopItem.WordId === item.WordId) {
            indexOfItemInArray = i;
            break;
        }
    }

    if (indexOfItemInArray !== -1) {
        shopList.splice(indexOfItemInArray, 1);
    }

    shopList.push(shopItem);
}

function loadShopCartView(shopList) {
    shopList = JSON.stringify(shopList);

    return $.ajax({
        url: "Store/Cart",
        type: "POST",
        contentType: "application/json",
        data: shopList
    });
}
