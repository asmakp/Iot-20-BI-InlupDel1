let tabledata = document.getElementById('tabledata')

setInterval(() => {
    tabledata.innerHTML = "";

    fetch("https://iot20-func.azurewebsites.net/api/GetDataFromCosmosDb")
        .then(res => res.json())
        .then(data => {

            for (let row of data) {
                tabledata.innerHTML += `<tr><td>${row.id}</td><td>${row.deviceId}</td><td>${row.data.Temperature}</td><td>${row.data.Humidity}</td><td>${row.data.MsgCreateTime}</td>`

            }
        })
}, 5000);