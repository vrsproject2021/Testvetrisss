// JavaScript Document

$(document).ready(function(){
    
    
    $("body").css("display", "none");
    $("body").fadeIn(2000); 
    // to fade out before redirect
    //$('').click(function(e){
    //    redirect = $(this).attr('');
    //    e.preventDefault();
    //    $('body').fadeOut(2000, function(){
    //        document.location.href = redirect
    //    });
    //});
    
})
function myFunction(x) {
   x.classList.toggle("change");
}
$(window).load(function () {
    $("body").on("focusout", '.input-effect input', function () {
	   
	        
			if($(this).val() != ""){

				$(this).addClass("has-content");
			} else {
			    if (!$(this).prop('readonly'))
				    $(this).removeClass("has-content");
			}
		})
	});