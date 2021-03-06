USE [GetMoney]
GO
/****** Object:  UserDefinedFunction [dbo].[SplitString]    Script Date: 11/21/2016 08:33:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[SplitString]
(
    @Input nvarchar(max),    @Separator nvarchar(max)=',', 
    @RemoveEmptyEntries bit=1 )
returns @TABLE table 
(
    [Id] int identity(1,1),
    [Value] nvarchar(max)
) 
as
begin 
    declare @Index int, @Entry nvarchar(max)
    set @Index = charindex(@Separator,@Input)

    while (@Index>0)
    begin
        set @Entry=ltrim(rtrim(substring(@Input, 1, @Index-1)))
        
        if (@RemoveEmptyEntries=0) or (@RemoveEmptyEntries=1 and @Entry<>'')
            begin
                insert into @TABLE([Value]) Values(@Entry)
            end

        set @Input = substring(@Input, @Index+datalength(@Separator)/2, len(@Input))
        set @Index = charindex(@Separator, @Input)
    end
    
    set @Entry=ltrim(rtrim(@Input))
    if (@RemoveEmptyEntries=0) or (@RemoveEmptyEntries=1 and @Entry<>'')
        begin
            insert into @TABLE([Value]) Values(@Entry)
        end

    return
end
GO
