

$('#CEP').blur(function () {
    value = this.value;
    $.ajax({
        type: "GET",
        url: "https://viacep.com.br/ws/" + value + "/json/",
        context: document.body,
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        $("#Logradouro").val(data.logradouro);
        $("#Bairro").val(data.bairro);
        $("#Cidade").val(data.localidade);
        $("#UF").val(data.uf);
    })
});