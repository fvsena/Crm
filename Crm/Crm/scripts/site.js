$('#Endereco_CEP').blur(function () {
    value = this.value;
    $.ajax({
        type: "GET",
        url: "https://viacep.com.br/ws/" + value + "/json/",
        context: document.body,
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        $("#Endereco_Logradouro").val(data.logradouro);
        $("#Endereco_Bairro").val(data.bairro);
        $("#Endereco_Cidade").val(data.localidade);
        $("#Endereco_UF").val(data.uf);
    })
});