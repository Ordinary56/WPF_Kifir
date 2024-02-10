"use strict";
//@ts-check

import { data } from "./main.js";


function Listaz() {

    const OsszesPontszamInput = document.getElementById('OsszesPontszamInput').value;
    const table = document.getElementById('dynamically_generated_table');

    table.querySelectorAll("th").forEach((headerrow) => {
        headerrow.addEventListener("click", (e) => {
            Filter(e.target);
        });
    });
    const SortByClick = (value) => {
        switch (value) {
            case "Neve":
            case "ErtesitesiCime":
            case "Email":
                data.sort((a, b) =>
                    a[value].localeCompare(b[value])
                );
                break;
            case "Matek":
            case "Magyar":
            case "OM_Azonosito":
            case "Összespontszam":
            data.sort((a, b) =>
                    a[value] - b[value]
                );
                break;
            case "SzuletesiDatum":
                data.sort((a, b) => {
                    new Date(a[value]) - new Date(b[value])
                });
                break;
        }
        Listaz(data);
    };
    const Filter = (headerrow) => {
        switch (headerrow.innerText) {
            case "Összes Pontszám":
                SortByClick("Összes Pontszám")
                break;
            case "Matek":
                SortByClick("Matematika");
                break;
            case "Magyar":
                SortByClick("Magyar");
                break;
            case "Email":
                SortByClick("Email");
                break;
            case "Neve":
                SortByClick("Neve");
                break;
            case "OM azonosító":
                SortByClick("OM_Azonosito");
                break;
            case "Értesítési Címe":
                SortByClick("ErtesitesiCime");
                break;
            case "Születési Dátum":
                SortByClick("SzuletesiDatum");
                break;
        }

    };

  

    // Check if OsszesPontszamInput is a valid number
    if (OsszesPontszamInput !== '' && !isNaN(OsszesPontszamInput)) {
        const existingTable = document.getElementById('dynamic-table');
        if (OsszesPontszamInput > 100) {
            return;
        }
        if (existingTable) {
            table.removeChild(existingTable);
        }

        const tableBody = document.createElement('tbody');
        tableBody.id = 'dynamic-table';

        // Add data rows
        const sortedData = data.filter(dataItem => dataItem.Matematika + dataItem.Magyar >= OsszesPontszamInput);

        sortedData.forEach(dataItem => {
            const dataRow = document.createElement('tr');

            const OM_AzonositoCell = document.createElement('td');
            OM_AzonositoCell.textContent = dataItem.OM_Azonosito;
            dataRow.appendChild(OM_AzonositoCell);

            const NeveCell = document.createElement('td');
            NeveCell.textContent = dataItem.Neve;
            dataRow.appendChild(NeveCell);

            const ErtesitesiCimeCell = document.createElement('td');
            ErtesitesiCimeCell.textContent = dataItem.ErtesitesiCime;
            dataRow.appendChild(ErtesitesiCimeCell);

            const EmailCell = document.createElement('td');
            EmailCell.textContent = dataItem.Email;
            dataRow.appendChild(EmailCell);

            const SzuletesiDatumCell = document.createElement('td');
            SzuletesiDatumCell.textContent = dataItem.SzuletesiDatum;
            dataRow.appendChild(SzuletesiDatumCell);

            const MatematikaCell = document.createElement('td');
            MatematikaCell.textContent = dataItem.Matematika;
            dataRow.appendChild(MatematikaCell);

            const MagyarCell = document.createElement('td');
            MagyarCell.textContent = dataItem.Magyar;
            dataRow.appendChild(MagyarCell);

            const OsszesPontszamCell = document.createElement('td');
            OsszesPontszamCell.textContent = dataItem.Matematika + dataItem.Magyar;
            dataRow.appendChild(OsszesPontszamCell);

            tableBody.appendChild(dataRow);
        });

        table.appendChild(tableBody);

    }
}

// Máskülönben nem találja a html
window.Listaz = Listaz;

//Feladom