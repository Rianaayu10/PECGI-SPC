/****** Object:  Table [dbo].[spc_Remark]    Script Date: 10/04/2023 09:52:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[spc_Remark](
	[RemarkType] [int] NOT NULL,
	[SeqNo] [int] NOT NULL,
	[Remark] [varchar](max) NULL,
 CONSTRAINT [PK_spc_Remark] PRIMARY KEY CLUSTERED 
(
	[RemarkType] ASC,
	[SeqNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

