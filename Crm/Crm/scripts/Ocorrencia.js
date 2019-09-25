$('#Grupos').change(function () {
    var select = document.getElementById("Grupos");
    var index = select.selectedIndex;
    var value = select[index].value;

    $.ajax({
        type: "GET",
        url: "ObterSubGrupos?IdGrupo=" + value,
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        var json = JSON.parse(data);
        $("#Detalhes").find('option').remove().end();
        $("#SubGrupos").find('option').remove().end();
        $("#Detalhes").append(new Option('--Selecione--', '0'));
        $("#SubGrupos").append(new Option('--Selecione--', '0'));
        $.each(json, function () {
            $("#SubGrupos").append(new Option(this.Nome, this.Id));
        });
    });
});

$('#SubGrupos').change(function () {
    var Gselect = document.getElementById("Grupos");
    var Gindex = Gselect.selectedIndex;
    var Gvalue = Gselect[Gindex].value;

    var Sselect = document.getElementById("SubGrupos");
    var Sindex = Sselect.selectedIndex;
    var Svalue = Sselect[Sindex].value;
    $.ajax({
        type: "GET",
        url: "ObterDetalhes?IdGrupo=" + Gvalue + "&IdSubGrupo=" + Svalue,
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        var json = JSON.parse(data);
        $("#Detalhes").find('option').remove().end();
        $("#Detalhes").append(new Option('--Selecione--', '0'));
        $.each(json, function () {
            $("#Detalhes").append(new Option(this.Nome, this.Id));
        });
    });
});