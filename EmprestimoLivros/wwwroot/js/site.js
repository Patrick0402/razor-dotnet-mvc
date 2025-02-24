$(function () {
    $('#Emprestimos').DataTable({
        language: {
            "decimal": "",
            "emptyTable": "Não há registros na tabela",
            "info": "Exibindo de _START_ a _END_ de _TOTAL_ registros",
            "infoEmpty": "Exibindo 0 de 0 registros",
            "infoFiltered": "(Filtrado de _MAX_ registros totais)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ registros",
            "loadingRecords": "Carregando...",
            "processing": "",
            "search": "Procurar:",
            "zeroRecords": "Nenhum registro encontrado",
            "paginate": {
                "first": "<<",
                "last": ">>",
                "next": ">",
                "previous": "<"
            },
            "aria": {
                "orderable": "Ordenar por esta coluna",
                "orderableReverse": "Ordenar reversamente por esta coluna"
            }
        }
    });

    setTimeout(function () {
        $(".alert").fadeOut("slow", function () {
            $(this).alert("close");
        });
    }, 5000);
});
