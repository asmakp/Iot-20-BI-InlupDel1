let tabledata = document.getElementById('tabledata')

setInterval(() => {
tabledata.innerHTML = "";

fetch("https://iot20-func.azurewebsites.net/api/GetDataFrnTbleStrg?code=a/uUH3sAzjEy4WSam7muaOAv53CMJsXap84QIEa0PYLhadqSGOXJ4A==")
    .then(res => res.json())
    .then(data => {
        //console.log(data);

        for (let row of data) {
            tabledata.innerHTML += `<tr><td>${row.rowKey}</td><td>${row.deviceId}</td><td>${row.distance}</td><td>${row.latitude}</td><td>${row.longitude}</td>`

        }
    })
    }, 5000);
