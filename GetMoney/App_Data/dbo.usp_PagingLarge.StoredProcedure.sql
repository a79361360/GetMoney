USE [GetMoney]
GO
/****** Object:  StoredProcedure [dbo].[usp_PagingLarge]    Script Date: 10/09/2016 09:51:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--EXEC usp_PagingLarge @TableNames='MLY_CodeRecord',@PrimaryKey='id',@Fields='',@PageSize=20,@CurrentPage=1,@Filter='',@Group='',@Order='id',@RecordCount=1

--EXEC usp_PagingLarge @TableNames='MLY_CodeRecord inner join t_member on cast(MLY_CodeRecord.Userid as varchar(50))=t_member.username',@PrimaryKey='MLY_CodeRecord.id',@Fields='',@PageSize=20,@CurrentPage=0,@Filter='',@Group='',@Order='MLY_CodeRecord.id',@RecordCount=1
Create PROCEDURE [dbo].[usp_PagingLarge]
@TableNames VARCHAR(200),     --表名，可以是多个表，但不能用别名
@PrimaryKey VARCHAR(100),     --主键，可以为空，但@Order为空时该值不能为空
@Fields     VARCHAR(4000),         --要取出的字段，可以是多个表的字段，可以为空，为空表示select *
@PageSize INT,             --每页记录数
@CurrentPage INT,         --当前页，0表示第1页
@Filter VARCHAR(4000) = '',     --条件，可以为空，不用填 where
@Group VARCHAR(200) = '',     --分组依据，可以为空，不用填 group by
@Order VARCHAR(200) = '',    --排序，可以为空，为空默认按主键升序排列，不用填 order by
@RecordCount int OUTPUT             --总记录数,自己增加（总记录数）
AS
BEGIN
     DECLARE @SortColumn VARCHAR(200)
     DECLARE @Operator CHAR(2)
     DECLARE @SortTable VARCHAR(200)
     DECLARE @SortName VARCHAR(200)
     IF @Fields = ''
         SET @Fields = '*'
     IF @Filter = ''
         SET @Filter = 'Where 1=1'
     ELSE
         SET @Filter = 'Where ' +   @Filter
     IF @Group <>''
         SET @Group = 'GROUP BY ' + @Group

     IF @Order <> ''
     BEGIN
         DECLARE @pos1 INT, @pos2 INT
         SET @Order = REPLACE(REPLACE(@Order, ' asc', ' ASC'), ' desc', ' DESC')
         IF CHARINDEX(' DESC', @Order) > 0
             IF CHARINDEX(' ASC', @Order) > 0
             BEGIN
                 IF CHARINDEX(' DESC', @Order) < CHARINDEX(' ASC', @Order)
                     SET @Operator = '<='
                 ELSE
                     SET @Operator = '>='
             END
             ELSE
                 SET @Operator = '<='
         ELSE
             SET @Operator = '>='
         SET @SortColumn = REPLACE(REPLACE(REPLACE(@Order, ' ASC', ''), ' DESC', ''), ' ', '')
         SET @pos1 = CHARINDEX(',', @SortColumn)
         IF @pos1 > 0
             SET @SortColumn = SUBSTRING(@SortColumn, 1, @pos1-1)
         SET @pos2 = CHARINDEX('.', @SortColumn)
         IF @pos2 > 0
         BEGIN
             SET @SortTable = SUBSTRING(@SortColumn, 1, @pos2-1)
             IF @pos1 > 0 
                 SET @SortName = SUBSTRING(@SortColumn, @pos2+1, @pos1-@pos2-1)
             ELSE
                 SET @SortName = SUBSTRING(@SortColumn, @pos2+1, LEN(@SortColumn)-@pos2)
         END
         ELSE
         BEGIN
             SET @SortTable = @TableNames
             SET @SortName = @SortColumn
         END
     END
     ELSE
     BEGIN
         SET @SortColumn = @PrimaryKey
         SET @SortTable = @TableNames
         SET @SortName = @SortColumn
         SET @Order = @SortColumn
         SET @Operator = '>='
     END

     DECLARE @type varchar(50)
     DECLARE @prec int
     Select @type=t.name, @prec=c.prec
     FROM sysobjects o 
     JOIN syscolumns c on o.id=c.id
     JOIN systypes t on c.xusertype=t.xusertype
     Where o.name = @SortTable AND c.name = @SortName
     IF CHARINDEX('char', @type) > 0
     SET @type = @type + '(' + CAST(@prec AS varchar) + ')'

     DECLARE @TopRows INT
     SET @TopRows = @PageSize * @CurrentPage + 1
     print @TopRows
     print @Operator
     EXEC('
         DECLARE @SortColumnBegin ' + @type + '
         SET ROWCOUNT ' + @TopRows + '
         Select @SortColumnBegin=' + @SortColumn + ' FROM   ' + @TableNames + ' ' + @Filter + ' ' + @Group + ' orDER BY ' + @Order + '
         SET ROWCOUNT ' + @PageSize + '
         Select ' + @Fields + ' FROM   ' + @TableNames + ' ' + @Filter   + ' AND ' + @SortColumn + '' + @Operator + '@SortColumnBegin ' + @Group + ' orDER BY ' + @Order + '    
     ')    
        IF @RecordCount IS NULL
BEGIN
    DECLARE @sql nvarchar(4000)
    SET @sql=N'SELECT @RecordCount=COUNT(*)'
        +N' FROM '+@TableNames
        +N' '+@Filter
    EXEC sp_executesql @sql,N'@RecordCount int OUTPUT',@RecordCount OUTPUT
END


END
GO
