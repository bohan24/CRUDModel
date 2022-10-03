$(document).ready(function () {
    GetData();
});

async function GetData() {
    var name = document.getElementById("custName").value;
    var birthday = document.getElementById("custBirth").value;
    var phone = document.getElementById("custPhone").value;
    var page = document.getElementById("page").value;
    var size = document.getElementById("size").value;
    let parameters = {
        url: "/Customer/getCustomer",
        type: "GET",
        data: {
            CustName: name,
            CustBirthday: birthday,
            CustPhone: phone,
            Page: page,
            PageSize:size
        }
    }

    const result = await CallAjax(parameters);

    if (!_.isEmpty(result)) {
        let str = '';
        document.getElementById("main").innerHTML = str;
        let { customer } = result;
        for (var i = 0; i < customer.length; i++) {
            str += `
            <tr>
                <th scope="row">${customer[i].custId}</th>
                <td>${customer[i].custName}</td>
                <td>${customer[i].custSex=="F"?"女":"男"}</td>
                <td>${customer[i].custBirthday}</td>
                <td>${customer[i].custPhone}</td>
                <td><button type="button" class="btn btn-info" onclick="toAiTable('${customer[i].custId}')">客戶資料移轉至AI</button></td>
            </tr>
            `;
        }
        document.getElementById("main").innerHTML = str;
    }
    else {
        swalTip("查詢失敗", "")
    }
}

async function toAiTable(no) {

    let parameters = {
        url: "/Customer/singleReturnCustomerToAi",
        type: "POST",
        data: {"CustId":no}
    }

    const result = await CallAjax(parameters);

    if (!_.isEmpty(result)) {
        console.log(result);
        alert("轉換成功");
    }
    else {
        swalTip("查詢失敗", "")
    }
}

function upPage() {
    //console.log("減少");
    document.getElementById("page").value == 1 ? null : document.getElementById("page").value--;
    GetData();
}

function nextPage() {
    //console.log("增加");
    document.getElementById("page").value++;
    GetData();
}