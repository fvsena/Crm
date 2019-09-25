$('#Grupos_Detalhe').change(function () {
    var select = document.getElementById("Grupos_Detalhe");
    var index = select.selectedIndex;
    var value = select[index].value;

    $.ajax({
        type: "GET",
        url: "ObterSubGrupos?IdGrupo="+value,
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        var json = JSON.parse(data);
        $("#SubGrupos").find('option').remove().end();
        $.each(json, function () {
            $("#SubGrupos").append(new Option(this.Nome, this.Id));
        });
    });
});