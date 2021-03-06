USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[day_end_accounts_posting_proc_status_update]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[day_end_accounts_posting_proc_status_update]
GO
/****** Object:  StoredProcedure [dbo].[day_end_accounts_posting_proc_status_update]    Script Date: 28-09-2021 19:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : day_end_accounts_posting_proc_status_update : 
				  update processing status of accounts posting data at day end
** Created By   : Pavel Guha
** Created On   : 28/12/2020
*******************************************************/
create procedure [dbo].[day_end_accounts_posting_proc_status_update]
	@day_end_date datetime,
	@error_msg nvarchar(500)='' output,
	@return_type int=0 output
as
begin
	set nocount on

	declare @srl_no int,
	        @ref_no nvarchar(20),
			@date_created datetime,
			@date_modified datetime,
			@date_txn datetime,
			@txn_id nvarchar(30),
			@txn_type nvarchar(10),
			@txn_ref_no nvarchar(30),
			@dr_cr_name nvarchar(100),
			@dr_cr_id uniqueidentifier,
			@gl_code nvarchar(5),
			@gl_desc nvarchar(100),
			@dr_amt money,
			@cr_amt money,
			@rowcount int,
			@counter int
	

	if(select count(day_end_date) from day_end_accounts_posting_processed where day_end_date=@day_end_date)=0
		begin
			begin transaction
			insert into day_end_accounts_posting_processed(day_end_date,record_count,process_completed)
					                                values(@day_end_date,0,'Y')
			if(@@rowcount=0)
				begin
					rollback transaction
					select @error_msg = 'Failed to update the day end accounts posting process status for the date ' + convert(varchar(10),@day_end_date,101),
							@return_type=0
					return 0
				end
			commit transaction
		end
	else
		begin
			select @error_msg = 'Day end accounts data already saved for the date ' + convert(varchar(10),@day_end_date,101),
			       @return_type=0
			return 0
		end
	

	set nocount off
	select @error_msg='',@return_type=1
	return 1

end

GO
