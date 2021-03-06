USE [F8EAF370D83155F0F88D4429043523B5_ 4 FEB--2013 AFTER 4  26 PM\CUSTOMER_CARE_SUPPORT_SYSTEM\APP_DATA\CUSTOMER_CARE_SUPORT_DATA.MDF]
GO
/****** Object:  Table [dbo].[MF_Client_Master]    Script Date: 04/04/2013 11:48:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MF_Client_Master](
	[clientid] [bigint] IDENTITY(1,1) NOT NULL,
	[clientname] [nvarchar](50) NULL,
	[clientalias] [nvarchar](50) NULL,
	[mobileno] [nvarchar](max) NULL,
	[landline] [nvarchar](max) NULL,
	[emailid1] [nvarchar](max) NULL,
	[emailid2] [nvarchar](max) NULL,
	[address1] [nvarchar](max) NULL,
	[address2] [nvarchar](max) NULL,
	[address3] [nvarchar](max) NULL,
	[city] [nvarchar](max) NULL,
	[panno] [nvarchar](max) NULL,
	[dob] [nvarchar](max) NULL,
	[groupname] [nvarchar](max) NULL,
	[groupalias] [nvarchar](max) NULL,
	[subbroker] [nvarchar](max) NULL,
	[rm] [nvarchar](max) NULL,
	[fileno] [nvarchar](max) NULL,
	[lock] [nvarchar](max) NULL,
	[equity] [nvarchar](max) NULL,
	[debt] [nvarchar](max) NULL,
	[equitycode1] [nvarchar](max) NULL,
	[equitycode2] [nvarchar](max) NULL,
	[insdate] [datetime] NULL,
	[update1] [datetime] NULL,
	[branch] [nvarchar](max) NULL,
 CONSTRAINT [PK_MF_Client_Master] PRIMARY KEY CLUSTERED 
(
	[clientid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
