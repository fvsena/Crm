function gravarCliente(cliente) {
    console.log(cliente);
    $.ajax({
        type: "POST",
        url: "GravarCliente",
        content: cliente,
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        console.log(data);
        alert("Cliente gravado com sucesso!");
    });
}

function selecionaCliente(codigo) {
    console.log(codigo);
    $.ajax({
        type: "GET",
        url: "/Cliente/SelecionaCliente?codigo="+codigo.getAttribute("value"),
        content: codigo,
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        console.log(data);
    });
}

function adicionarTelefone() {
    var val = $("#txtTelefone").val();
    console.log(val);
    if (val.length > 0) {
        $("#Telefones").append(new Option(val, val));
        $("#txtTelefone").val('');
    }
}