using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.Issues {
	/// <summary>
	/// This class attempts to create the error associated with Issue 174. Unable to replicate
	/// the error at the moment.
	/// </summary>
	public class Issue174 {
		/// <summary>
		///Represents the connection and commands used to retrieve and save data.
		///</summary>
		[global::System.ComponentModel.DesignerCategoryAttribute("code")]
		[global::System.ComponentModel.ToolboxItem(true)]
		[global::System.ComponentModel.DataObjectAttribute(true)]
		[global::System.ComponentModel.DesignerAttribute("Microsoft.VSDesigner.DataSource.Design.TableAdapterDesigner, Microsoft.VSDesigner" +
			", Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[global::System.ComponentModel.Design.HelpKeywordAttribute("vs.data.TableAdapter")]
		public partial class LessonTableAdapter : global::System.ComponentModel.Component {

			private global::System.Data.SqlClient.SqlDataAdapter _adapter;

			private global::System.Data.SqlClient.SqlConnection _connection;

			private global::System.Data.SqlClient.SqlTransaction _transaction;

			private global::System.Data.SqlClient.SqlCommand[] _commandCollection;

			private bool _clearBeforeFill;

			[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public LessonTableAdapter() {
				this.ClearBeforeFill = true;
			}

			[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			protected internal global::System.Data.SqlClient.SqlDataAdapter Adapter {
				get {
					if ((this._adapter == null)) {
						this.InitAdapter();
					}
					return this._adapter;
				}
			}

			[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			internal global::System.Data.SqlClient.SqlConnection Connection {
				get {
					if ((this._connection == null)) {
						//this.InitConnection();
					}
					return this._connection;
				}
				set {
					this._connection = value;
					if ((this.Adapter.InsertCommand != null)) {
						this.Adapter.InsertCommand.Connection = value;
					}
					if ((this.Adapter.DeleteCommand != null)) {
						this.Adapter.DeleteCommand.Connection = value;
					}
					if ((this.Adapter.UpdateCommand != null)) {
						this.Adapter.UpdateCommand.Connection = value;
					}
					for (int i = 0; (i < this.CommandCollection.Length); i = (i + 1)) {
						if ((this.CommandCollection[i] != null)) {
							((global::System.Data.SqlClient.SqlCommand)(this.CommandCollection[i])).Connection = value;
						}
					}
				}
			}

			[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			internal global::System.Data.SqlClient.SqlTransaction Transaction {
				get {
					return this._transaction;
				}
				set {
					this._transaction = value;
					for (int i = 0; (i < this.CommandCollection.Length); i = (i + 1)) {
						this.CommandCollection[i].Transaction = this._transaction;
					}
					if (((this.Adapter != null)
								&& (this.Adapter.DeleteCommand != null))) {
						this.Adapter.DeleteCommand.Transaction = this._transaction;
					}
					if (((this.Adapter != null)
								&& (this.Adapter.InsertCommand != null))) {
						this.Adapter.InsertCommand.Transaction = this._transaction;
					}
					if (((this.Adapter != null)
								&& (this.Adapter.UpdateCommand != null))) {
						this.Adapter.UpdateCommand.Transaction = this._transaction;
					}
				}
			}

			[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			protected global::System.Data.SqlClient.SqlCommand[] CommandCollection {
				get {
					if ((this._commandCollection == null)) {
						this.InitCommandCollection();
					}
					return this._commandCollection;
				}
			}

			[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public bool ClearBeforeFill {
				get {
					return this._clearBeforeFill;
				}
				set {
					this._clearBeforeFill = value;
				}
			}

			[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			private void InitAdapter() {
				this._adapter = new global::System.Data.SqlClient.SqlDataAdapter();
				global::System.Data.Common.DataTableMapping tableMapping = new global::System.Data.Common.DataTableMapping();
				tableMapping.SourceTable = "Table";
				tableMapping.DataSetTable = "Lesson";
				tableMapping.ColumnMappings.Add("LessonId", "LessonId");
				tableMapping.ColumnMappings.Add("Teacher", "Teacher");
				tableMapping.ColumnMappings.Add("DateEntered", "DateEntered");
				tableMapping.ColumnMappings.Add("QAType", "QAType");
				tableMapping.ColumnMappings.Add("ObservationType", "ObservationType");
				tableMapping.ColumnMappings.Add("Length", "Length");
				tableMapping.ColumnMappings.Add("Observer", "Observer");
				tableMapping.ColumnMappings.Add("YearGroup", "YearGroup");
				tableMapping.ColumnMappings.Add("ClassCode", "ClassCode");
				tableMapping.ColumnMappings.Add("Boys", "Boys");
				tableMapping.ColumnMappings.Add("Girls", "Girls");
				tableMapping.ColumnMappings.Add("Subject", "Subject");
				tableMapping.ColumnMappings.Add("SetNo", "SetNo");
				tableMapping.ColumnMappings.Add("SetOf", "SetOf");
				tableMapping.ColumnMappings.Add("AssessmentRangeA", "AssessmentRangeA");
				tableMapping.ColumnMappings.Add("AssessmentRangeB", "AssessmentRangeB");
				tableMapping.ColumnMappings.Add("TimeOfDay", "TimeOfDay");
				tableMapping.ColumnMappings.Add("LessonPeriod", "LessonPeriod");
				tableMapping.ColumnMappings.Add("Support", "Support");
				tableMapping.ColumnMappings.Add("FileName", "FileName");
				tableMapping.ColumnMappings.Add("BookNo", "BookNo");
				tableMapping.ColumnMappings.Add("PageNo", "PageNo");
				tableMapping.ColumnMappings.Add("FormName", "FormName");
				tableMapping.ColumnMappings.Add("FormNo", "FormNo");
				tableMapping.ColumnMappings.Add("PenId", "PenId");
				tableMapping.ColumnMappings.Add("QaSignature", "QaSignature");
				tableMapping.ColumnMappings.Add("StaffSignature", "StaffSignature");
				tableMapping.ColumnMappings.Add("OverallJudgment", "OverallJudgment");
				tableMapping.ColumnMappings.Add("OJComments", "OJComments");
				tableMapping.ColumnMappings.Add("DocumentName", "DocumentName");
				tableMapping.ColumnMappings.Add("FollowUp", "FollowUp");
				tableMapping.ColumnMappings.Add("Validated", "Validated");
				tableMapping.ColumnMappings.Add("LogicallyDeleted", "LogicallyDeleted");
				tableMapping.ColumnMappings.Add("SummativeFutureDevelopment", "SummativeFutureDevelopment");
				tableMapping.ColumnMappings.Add("SummativeDevelopmentStrategyText", "SummativeDevelopmentStrategyText");
				tableMapping.ColumnMappings.Add("SummativeStrategyBeReviewed", "SummativeStrategyBeReviewed");
				tableMapping.ColumnMappings.Add("SummativeStrategyBeReviewedBy", "SummativeStrategyBeReviewedBy");
				tableMapping.ColumnMappings.Add("TargetsFutureDevelopment", "TargetsFutureDevelopment");
				tableMapping.ColumnMappings.Add("TargetsBeReviewed", "TargetsBeReviewed");
				tableMapping.ColumnMappings.Add("TargetsBeAchieved", "TargetsBeAchieved");
				tableMapping.ColumnMappings.Add("DateForFollowUp", "DateForFollowUp");
				this._adapter.TableMappings.Add(tableMapping);
				this._adapter.DeleteCommand = new global::System.Data.SqlClient.SqlCommand();
				this._adapter.DeleteCommand.Connection = this.Connection;
				this._adapter.DeleteCommand.CommandText = "DELETE FROM [Lesson] WHERE (([LessonId] = @Original_LessonId))";
				this._adapter.DeleteCommand.CommandType = global::System.Data.CommandType.Text;
				this._adapter.DeleteCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Original_LessonId", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "LessonId", global::System.Data.DataRowVersion.Original, false, null, "", "", ""));
				this._adapter.InsertCommand = new global::System.Data.SqlClient.SqlCommand();
				this._adapter.InsertCommand.Connection = this.Connection;
				this._adapter.InsertCommand.CommandText = "INSERT INTO [Lesson] ([Teacher], [DateEntered], [QAType], [ObservationType], [Len" +
					"gth], [Observer], [YearGroup], [ClassCode], [Boys], [Girls], [Subject], [SetNo]," +
					" [SetOf], [AssessmentRangeA], [AssessmentRangeB], [TimeOfDay], [LessonPeriod], [" +
					"Support], [FileName], [BookNo], [PageNo], [FormName], [FormNo], [PenId], [QaSign" +
					"ature], [StaffSignature], [OverallJudgment], [OJComments], [DocumentName], [Foll" +
					"owUp], [Validated], [LogicallyDeleted], [SummativeFutureDevelopment], [Summative" +
					"DevelopmentStrategyText], [SummativeStrategyBeReviewed], [SummativeStrategyBeRev" +
					"iewedBy], [TargetsFutureDevelopment], [TargetsBeReviewed], [TargetsBeAchieved], " +
					"[DateForFollowUp]) VALUES (@Teacher, @DateEntered, @QAType, @ObservationType, @L" +
					"ength, @Observer, @YearGroup, @ClassCode, @Boys, @Girls, @Subject, @SetNo, @SetO" +
					"f, @AssessmentRangeA, @AssessmentRangeB, @TimeOfDay, @LessonPeriod, @Support, @F" +
					"ileName, @BookNo, @PageNo, @FormName, @FormNo, @PenId, @QaSignature, @StaffSigna" +
					"ture, @OverallJudgment, @OJComments, @DocumentName, @FollowUp, @Validated, @Logi" +
					"callyDeleted, @SummativeFutureDevelopment, @SummativeDevelopmentStrategyText, @S" +
					"ummativeStrategyBeReviewed, @SummativeStrategyBeReviewedBy, @TargetsFutureDevelo" +
					"pment, @TargetsBeReviewed, @TargetsBeAchieved, @DateForFollowUp);\r\nSELECT Lesson" +
					"Id, Teacher, DateEntered, QAType, ObservationType, Length, Observer, YearGroup, " +
					"ClassCode, Boys, Girls, Subject, SetNo, SetOf, AssessmentRangeA, AssessmentRange" +
					"B, TimeOfDay, LessonPeriod, Support, FileName, BookNo, PageNo, FormName, FormNo," +
					" PenId, QaSignature, StaffSignature, OverallJudgment, OJComments, DocumentName, " +
					"FollowUp, Validated, LogicallyDeleted, SummativeFutureDevelopment, SummativeDeve" +
					"lopmentStrategyText, SummativeStrategyBeReviewed, SummativeStrategyBeReviewedBy," +
					" TargetsFutureDevelopment, TargetsBeReviewed, TargetsBeAchieved, DateForFollowUp" +
					" FROM Lesson WHERE (LessonId = SCOPE_IDENTITY())";
				this._adapter.InsertCommand.CommandType = global::System.Data.CommandType.Text;
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Teacher", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "Teacher", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@DateEntered", global::System.Data.SqlDbType.DateTime, 0, global::System.Data.ParameterDirection.Input, 0, 0, "DateEntered", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@QAType", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "QAType", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@ObservationType", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "ObservationType", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Length", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "Length", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Observer", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "Observer", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@YearGroup", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "YearGroup", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@ClassCode", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "ClassCode", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Boys", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "Boys", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Girls", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "Girls", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Subject", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "Subject", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@SetNo", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "SetNo", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@SetOf", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "SetOf", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@AssessmentRangeA", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "AssessmentRangeA", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@AssessmentRangeB", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "AssessmentRangeB", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@TimeOfDay", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "TimeOfDay", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@LessonPeriod", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "LessonPeriod", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Support", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "Support", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@FileName", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "FileName", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@BookNo", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "BookNo", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@PageNo", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "PageNo", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@FormName", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "FormName", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@FormNo", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "FormNo", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@PenId", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "PenId", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@QaSignature", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "QaSignature", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@StaffSignature", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "StaffSignature", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@OverallJudgment", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "OverallJudgment", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@OJComments", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "OJComments", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@DocumentName", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "DocumentName", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@FollowUp", global::System.Data.SqlDbType.Bit, 0, global::System.Data.ParameterDirection.Input, 0, 0, "FollowUp", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Validated", global::System.Data.SqlDbType.Bit, 0, global::System.Data.ParameterDirection.Input, 0, 0, "Validated", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@LogicallyDeleted", global::System.Data.SqlDbType.Bit, 0, global::System.Data.ParameterDirection.Input, 0, 0, "LogicallyDeleted", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@SummativeFutureDevelopment", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "SummativeFutureDevelopment", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@SummativeDevelopmentStrategyText", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "SummativeDevelopmentStrategyText", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@SummativeStrategyBeReviewed", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "SummativeStrategyBeReviewed", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@SummativeStrategyBeReviewedBy", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "SummativeStrategyBeReviewedBy", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@TargetsFutureDevelopment", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "TargetsFutureDevelopment", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@TargetsBeReviewed", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "TargetsBeReviewed", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@TargetsBeAchieved", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "TargetsBeAchieved", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.InsertCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@DateForFollowUp", global::System.Data.SqlDbType.DateTime, 0, global::System.Data.ParameterDirection.Input, 0, 0, "DateForFollowUp", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand = new global::System.Data.SqlClient.SqlCommand();
				this._adapter.UpdateCommand.Connection = this.Connection;
				this._adapter.UpdateCommand.CommandText = "UPDATE [Lesson] SET [Teacher] = @Teacher, [DateEntered] = @DateEntered, [QAType] " +
					"= @QAType, [ObservationType] = @ObservationType, [Length] = @Length, [Observer] " +
					"= @Observer, [YearGroup] = @YearGroup, [ClassCode] = @ClassCode, [Boys] = @Boys," +
					" [Girls] = @Girls, [Subject] = @Subject, [SetNo] = @SetNo, [SetOf] = @SetOf, [As" +
					"sessmentRangeA] = @AssessmentRangeA, [AssessmentRangeB] = @AssessmentRangeB, [Ti" +
					"meOfDay] = @TimeOfDay, [LessonPeriod] = @LessonPeriod, [Support] = @Support, [Fi" +
					"leName] = @FileName, [BookNo] = @BookNo, [PageNo] = @PageNo, [FormName] = @FormN" +
					"ame, [FormNo] = @FormNo, [PenId] = @PenId, [QaSignature] = @QaSignature, [StaffS" +
					"ignature] = @StaffSignature, [OverallJudgment] = @OverallJudgment, [OJComments] " +
					"= @OJComments, [DocumentName] = @DocumentName, [FollowUp] = @FollowUp, [Validate" +
					"d] = @Validated, [LogicallyDeleted] = @LogicallyDeleted, [SummativeFutureDevelop" +
					"ment] = @SummativeFutureDevelopment, [SummativeDevelopmentStrategyText] = @Summa" +
					"tiveDevelopmentStrategyText, [SummativeStrategyBeReviewed] = @SummativeStrategyB" +
					"eReviewed, [SummativeStrategyBeReviewedBy] = @SummativeStrategyBeReviewedBy, [Ta" +
					"rgetsFutureDevelopment] = @TargetsFutureDevelopment, [TargetsBeReviewed] = @Targ" +
					"etsBeReviewed, [TargetsBeAchieved] = @TargetsBeAchieved, [DateForFollowUp] = @Da" +
					"teForFollowUp WHERE (([LessonId] = @Original_LessonId));\r\nSELECT LessonId, Teach" +
					"er, DateEntered, QAType, ObservationType, Length, Observer, YearGroup, ClassCode" +
					", Boys, Girls, Subject, SetNo, SetOf, AssessmentRangeA, AssessmentRangeB, TimeOf" +
					"Day, LessonPeriod, Support, FileName, BookNo, PageNo, FormName, FormNo, PenId, Q" +
					"aSignature, StaffSignature, OverallJudgment, OJComments, DocumentName, FollowUp," +
					" Validated, LogicallyDeleted, SummativeFutureDevelopment, SummativeDevelopmentSt" +
					"rategyText, SummativeStrategyBeReviewed, SummativeStrategyBeReviewedBy, TargetsF" +
					"utureDevelopment, TargetsBeReviewed, TargetsBeAchieved, DateForFollowUp FROM Les" +
					"son WHERE (LessonId = @LessonId)";
				this._adapter.UpdateCommand.CommandType = global::System.Data.CommandType.Text;
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Teacher", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "Teacher", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@DateEntered", global::System.Data.SqlDbType.DateTime, 0, global::System.Data.ParameterDirection.Input, 0, 0, "DateEntered", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@QAType", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "QAType", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@ObservationType", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "ObservationType", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Length", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "Length", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Observer", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "Observer", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@YearGroup", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "YearGroup", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@ClassCode", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "ClassCode", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Boys", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "Boys", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Girls", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "Girls", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Subject", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "Subject", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@SetNo", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "SetNo", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@SetOf", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "SetOf", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@AssessmentRangeA", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "AssessmentRangeA", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@AssessmentRangeB", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "AssessmentRangeB", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@TimeOfDay", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "TimeOfDay", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@LessonPeriod", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "LessonPeriod", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Support", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "Support", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@FileName", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "FileName", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@BookNo", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "BookNo", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@PageNo", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "PageNo", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@FormName", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "FormName", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@FormNo", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "FormNo", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@PenId", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "PenId", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@QaSignature", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "QaSignature", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@StaffSignature", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "StaffSignature", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@OverallJudgment", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "OverallJudgment", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@OJComments", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "OJComments", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@DocumentName", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "DocumentName", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@FollowUp", global::System.Data.SqlDbType.Bit, 0, global::System.Data.ParameterDirection.Input, 0, 0, "FollowUp", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Validated", global::System.Data.SqlDbType.Bit, 0, global::System.Data.ParameterDirection.Input, 0, 0, "Validated", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@LogicallyDeleted", global::System.Data.SqlDbType.Bit, 0, global::System.Data.ParameterDirection.Input, 0, 0, "LogicallyDeleted", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@SummativeFutureDevelopment", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "SummativeFutureDevelopment", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@SummativeDevelopmentStrategyText", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "SummativeDevelopmentStrategyText", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@SummativeStrategyBeReviewed", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "SummativeStrategyBeReviewed", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@SummativeStrategyBeReviewedBy", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "SummativeStrategyBeReviewedBy", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@TargetsFutureDevelopment", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "TargetsFutureDevelopment", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@TargetsBeReviewed", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "TargetsBeReviewed", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@TargetsBeAchieved", global::System.Data.SqlDbType.NVarChar, 0, global::System.Data.ParameterDirection.Input, 0, 0, "TargetsBeAchieved", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@DateForFollowUp", global::System.Data.SqlDbType.DateTime, 0, global::System.Data.ParameterDirection.Input, 0, 0, "DateForFollowUp", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Original_LessonId", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "LessonId", global::System.Data.DataRowVersion.Original, false, null, "", "", ""));
				this._adapter.UpdateCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@LessonId", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.Input, 0, 0, "LessonId", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
			}

			//[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			//private void InitConnection() {
			//    this._connection = new global::System.Data.SqlClient.SqlConnection();
			//    this._connection.ConnectionString = global::Ucst.Data.Properties.Settings.Default.LessonObservationsConnectionString;
			//}

			[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			private void InitCommandCollection() {
				this._commandCollection = new global::System.Data.SqlClient.SqlCommand[5];
				this._commandCollection[0] = new global::System.Data.SqlClient.SqlCommand();
				this._commandCollection[0].Connection = this.Connection;
				this._commandCollection[0].CommandText = @"SELECT        LessonId, Teacher, DateEntered, QAType, ObservationType, Length, Observer, YearGroup, ClassCode, Boys, Girls, Subject, SetNo, SetOf, AssessmentRangeA, 
                         AssessmentRangeB, TimeOfDay, LessonPeriod, Support, FileName, BookNo, PageNo, FormName, FormNo, PenId, QaSignature, StaffSignature, OverallJudgment, 
                         OJComments, DocumentName, FollowUp, Validated, LogicallyDeleted, SummativeFutureDevelopment, SummativeDevelopmentStrategyText, 
                         SummativeStrategyBeReviewed, SummativeStrategyBeReviewedBy, TargetsFutureDevelopment, TargetsBeReviewed, TargetsBeAchieved, DateForFollowUp
FROM            Lesson";
				this._commandCollection[0].CommandType = global::System.Data.CommandType.Text;
				this._commandCollection[1] = new global::System.Data.SqlClient.SqlCommand();
				this._commandCollection[1].Connection = this.Connection;
				this._commandCollection[1].CommandText = @"SELECT AssessmentRangeA, AssessmentRangeB, BookNo, Boys, ClassCode, DateEntered, DateForFollowUp, DocumentName, FileName, FollowUp, FormName, FormNo, Girls, Length, LessonId, LessonPeriod, LogicallyDeleted, OJComments, ObservationType, Observer, OverallJudgment, PageNo, PenId, QAType, QaSignature, SetNo, SetOf, StaffSignature, Subject, SummativeDevelopmentStrategyText, SummativeFutureDevelopment, SummativeStrategyBeReviewed, SummativeStrategyBeReviewedBy, Support, TargetsBeAchieved, TargetsBeReviewed, TargetsFutureDevelopment, Teacher, TimeOfDay, Validated, YearGroup FROM Lesson WHERE (LessonId = @LessonId)";
				this._commandCollection[1].CommandType = global::System.Data.CommandType.Text;
				this._commandCollection[1].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@LessonId", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.Input, 0, 0, "LessonId", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2] = new global::System.Data.SqlClient.SqlCommand();
				this._commandCollection[2].Connection = this.Connection;
				this._commandCollection[2].CommandText = "dbo.Lesson_Insert";
				this._commandCollection[2].CommandType = global::System.Data.CommandType.StoredProcedure;
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@RETURN_VALUE", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.ReturnValue, 10, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@LessonId", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.InputOutput, 10, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Teacher", global::System.Data.SqlDbType.NVarChar, 4, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@DateEntered", global::System.Data.SqlDbType.DateTime, 8, global::System.Data.ParameterDirection.Input, 23, 3, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@QAType", global::System.Data.SqlDbType.NVarChar, 10, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@ObservationType", global::System.Data.SqlDbType.NVarChar, 10, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Length", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.Input, 10, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Observer", global::System.Data.SqlDbType.NVarChar, 4, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@YearGroup", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.Input, 10, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@ClassCode", global::System.Data.SqlDbType.NVarChar, 8, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Boys", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.Input, 10, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Girls", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.Input, 10, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Subject", global::System.Data.SqlDbType.NVarChar, 32, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@SetNo", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.Input, 10, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@SetOf", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.Input, 10, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@AssessmentRangeA", global::System.Data.SqlDbType.NVarChar, 5, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@AssessmentRangeB", global::System.Data.SqlDbType.NVarChar, 5, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@TimeOfDay", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.Input, 10, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@LessonPeriod", global::System.Data.SqlDbType.NVarChar, 3, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Support", global::System.Data.SqlDbType.NVarChar, 4, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@FileName", global::System.Data.SqlDbType.NVarChar, 80, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@BookNo", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.Input, 10, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@PageNo", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.Input, 10, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@FormName", global::System.Data.SqlDbType.NVarChar, 50, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@FormNo", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.Input, 10, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@PenId", global::System.Data.SqlDbType.NVarChar, 20, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@QASignature", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.Input, 10, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@StaffSignature", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.Input, 10, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@OverallJudgment", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.Input, 10, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@OJComments", global::System.Data.SqlDbType.NVarChar, 500, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@DocumentName", global::System.Data.SqlDbType.NVarChar, 80, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@FollowUp", global::System.Data.SqlDbType.Bit, 1, global::System.Data.ParameterDirection.Input, 1, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@DateForFollowUp", global::System.Data.SqlDbType.DateTime, 8, global::System.Data.ParameterDirection.Input, 23, 3, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@SummativeFutureDevelopment", global::System.Data.SqlDbType.NVarChar, 400, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@SummativeDevelopmentStrategyText", global::System.Data.SqlDbType.NVarChar, 200, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@SummativeStrategyBeReviewed", global::System.Data.SqlDbType.NVarChar, 100, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@SummativeStrategyBeReviewedBy", global::System.Data.SqlDbType.NVarChar, 80, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@TargetsFutureDevelopment", global::System.Data.SqlDbType.NVarChar, 400, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@TargetsBeAchieved", global::System.Data.SqlDbType.NVarChar, 200, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[2].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@TargetsBeReviewed", global::System.Data.SqlDbType.NVarChar, 100, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[3] = new global::System.Data.SqlClient.SqlCommand();
				this._commandCollection[3].Connection = this.Connection;
				this._commandCollection[3].CommandText = "dbo.Lesson_Update";
				this._commandCollection[3].CommandType = global::System.Data.CommandType.StoredProcedure;
				this._commandCollection[3].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@RETURN_VALUE", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.ReturnValue, 10, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[3].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@LessonId", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.Input, 10, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[3].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Teacher", global::System.Data.SqlDbType.NVarChar, 4, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[3].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@DateEntered", global::System.Data.SqlDbType.DateTime, 8, global::System.Data.ParameterDirection.Input, 23, 3, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[3].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@QAType", global::System.Data.SqlDbType.NVarChar, 10, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[3].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@ObservationType", global::System.Data.SqlDbType.NVarChar, 10, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[3].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Length", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.Input, 10, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[3].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Observer", global::System.Data.SqlDbType.NVarChar, 4, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[3].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@YearGroup", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.Input, 10, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[3].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@ClassCode", global::System.Data.SqlDbType.NVarChar, 8, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[3].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Boys", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.Input, 10, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[3].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Girls", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.Input, 10, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[3].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Subject", global::System.Data.SqlDbType.NVarChar, 32, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[3].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@SetNo", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.Input, 10, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[3].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@SetOf", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.Input, 10, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[3].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@AssessmentRangeA", global::System.Data.SqlDbType.NVarChar, 5, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[3].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@AssessmentRangeB", global::System.Data.SqlDbType.NVarChar, 5, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[3].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@TimeOfDay", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.Input, 10, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[3].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@LessonPeriod", global::System.Data.SqlDbType.NVarChar, 3, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[3].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Support", global::System.Data.SqlDbType.NVarChar, 4, global::System.Data.ParameterDirection.Input, 0, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[3].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@OverallJudgment", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.Input, 10, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[3].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@FollowUp", global::System.Data.SqlDbType.Bit, 1, global::System.Data.ParameterDirection.Input, 1, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[3].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@DateForFollowUp", global::System.Data.SqlDbType.DateTime, 8, global::System.Data.ParameterDirection.Input, 23, 3, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[3].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Validated", global::System.Data.SqlDbType.Bit, 1, global::System.Data.ParameterDirection.Input, 1, 0, null, global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[4] = new global::System.Data.SqlClient.SqlCommand();
				this._commandCollection[4].Connection = this.Connection;
				this._commandCollection[4].CommandText = "UPDATE    dbo.Lesson\r\nSET              Validated = @Validated\r\nWHERE     (LessonI" +
					"d = @LessonId)";
				this._commandCollection[4].CommandType = global::System.Data.CommandType.Text;
				this._commandCollection[4].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@Validated", global::System.Data.SqlDbType.Bit, 1, global::System.Data.ParameterDirection.Input, 0, 0, "Validated", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
				this._commandCollection[4].Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@LessonId", global::System.Data.SqlDbType.Int, 4, global::System.Data.ParameterDirection.Input, 0, 0, "LessonId", global::System.Data.DataRowVersion.Original, false, null, "", "", ""));
			}

			//[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			//[global::System.ComponentModel.Design.HelpKeywordAttribute("vs.data.TableAdapter")]
			//[global::System.ComponentModel.DataObjectMethodAttribute(global::System.ComponentModel.DataObjectMethodType.Select, true)]
			//public virtual LessonObservationDS.LessonDataTable GetData() {
			//    this.Adapter.SelectCommand = this.CommandCollection[0];
			//    LessonObservationDS.LessonDataTable dataTable = new LessonObservationDS.LessonDataTable();
			//    this.Adapter.Fill(dataTable);
			//    return dataTable;
			//}

			//[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			//[global::System.ComponentModel.Design.HelpKeywordAttribute("vs.data.TableAdapter")]
			//[global::System.ComponentModel.DataObjectMethodAttribute(global::System.ComponentModel.DataObjectMethodType.Select, false)]
			//public virtual LessonObservationDS.LessonDataTable GetById(int LessonId) {
			//    this.Adapter.SelectCommand = this.CommandCollection[1];
			//    this.Adapter.SelectCommand.Parameters[0].Value = ((int)(LessonId));
			//    LessonObservationDS.LessonDataTable dataTable = new LessonObservationDS.LessonDataTable();
			//    this.Adapter.Fill(dataTable);
			//    return dataTable;
			//}

			//[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			//[global::System.ComponentModel.Design.HelpKeywordAttribute("vs.data.TableAdapter")]
			//public virtual int Update(LessonObservationDS.LessonDataTable dataTable) {
			//    return this.Adapter.Update(dataTable);
			//}

			//[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			//[global::System.ComponentModel.Design.HelpKeywordAttribute("vs.data.TableAdapter")]
			//public virtual int Update(LessonObservationDS dataSet) {
			//    return this.Adapter.Update(dataSet, "Lesson");
			//}

			[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[global::System.ComponentModel.Design.HelpKeywordAttribute("vs.data.TableAdapter")]
			public virtual int Update(global::System.Data.DataRow dataRow) {
				return this.Adapter.Update(new global::System.Data.DataRow[] {
                        dataRow});
			}

			[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[global::System.ComponentModel.Design.HelpKeywordAttribute("vs.data.TableAdapter")]
			public virtual int Update(global::System.Data.DataRow[] dataRows) {
				return this.Adapter.Update(dataRows);
			}

			[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[global::System.ComponentModel.Design.HelpKeywordAttribute("vs.data.TableAdapter")]
			public virtual object InsertRecord(
						ref global::System.Nullable<int> LessonId,
						string Teacher,
						global::System.Nullable<global::System.DateTime> DateEntered,
						string QAType,
						string ObservationType,
						global::System.Nullable<int> Length,
						string Observer,
						global::System.Nullable<int> YearGroup,
						string ClassCode,
						global::System.Nullable<int> Boys,
						global::System.Nullable<int> Girls,
						string Subject,
						global::System.Nullable<int> SetNo,
						global::System.Nullable<int> SetOf,
						string AssessmentRangeA,
						string AssessmentRangeB,
						global::System.Nullable<int> TimeOfDay,
						string LessonPeriod,
						string Support,
						string FileName,
						global::System.Nullable<int> BookNo,
						global::System.Nullable<int> PageNo,
						string FormName,
						global::System.Nullable<int> FormNo,
						string PenId,
						global::System.Nullable<int> QASignature,
						global::System.Nullable<int> StaffSignature,
						global::System.Nullable<int> OverallJudgment,
						string OJComments,
						string DocumentName,
						global::System.Nullable<bool> FollowUp,
						global::System.Nullable<global::System.DateTime> DateForFollowUp,
						string SummativeFutureDevelopment,
						string SummativeDevelopmentStrategyText,
						string SummativeStrategyBeReviewed,
						string SummativeStrategyBeReviewedBy,
						string TargetsFutureDevelopment,
						string TargetsBeAchieved,
						string TargetsBeReviewed) {
				global::System.Data.SqlClient.SqlCommand command = this.CommandCollection[2];
				if ((LessonId.HasValue == true)) {
					command.Parameters[1].Value = ((int)(LessonId.Value));
				}
				else {
					command.Parameters[1].Value = global::System.DBNull.Value;
				}
				if ((Teacher == null)) {
					command.Parameters[2].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[2].Value = ((string)(Teacher));
				}
				if ((DateEntered.HasValue == true)) {
					command.Parameters[3].Value = ((System.DateTime)(DateEntered.Value));
				}
				else {
					command.Parameters[3].Value = global::System.DBNull.Value;
				}
				if ((QAType == null)) {
					command.Parameters[4].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[4].Value = ((string)(QAType));
				}
				if ((ObservationType == null)) {
					command.Parameters[5].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[5].Value = ((string)(ObservationType));
				}
				if ((Length.HasValue == true)) {
					command.Parameters[6].Value = ((int)(Length.Value));
				}
				else {
					command.Parameters[6].Value = global::System.DBNull.Value;
				}
				if ((Observer == null)) {
					command.Parameters[7].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[7].Value = ((string)(Observer));
				}
				if ((YearGroup.HasValue == true)) {
					command.Parameters[8].Value = ((int)(YearGroup.Value));
				}
				else {
					command.Parameters[8].Value = global::System.DBNull.Value;
				}
				if ((ClassCode == null)) {
					command.Parameters[9].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[9].Value = ((string)(ClassCode));
				}
				if ((Boys.HasValue == true)) {
					command.Parameters[10].Value = ((int)(Boys.Value));
				}
				else {
					command.Parameters[10].Value = global::System.DBNull.Value;
				}
				if ((Girls.HasValue == true)) {
					command.Parameters[11].Value = ((int)(Girls.Value));
				}
				else {
					command.Parameters[11].Value = global::System.DBNull.Value;
				}
				if ((Subject == null)) {
					command.Parameters[12].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[12].Value = ((string)(Subject));
				}
				if ((SetNo.HasValue == true)) {
					command.Parameters[13].Value = ((int)(SetNo.Value));
				}
				else {
					command.Parameters[13].Value = global::System.DBNull.Value;
				}
				if ((SetOf.HasValue == true)) {
					command.Parameters[14].Value = ((int)(SetOf.Value));
				}
				else {
					command.Parameters[14].Value = global::System.DBNull.Value;
				}
				if ((AssessmentRangeA == null)) {
					command.Parameters[15].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[15].Value = ((string)(AssessmentRangeA));
				}
				if ((AssessmentRangeB == null)) {
					command.Parameters[16].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[16].Value = ((string)(AssessmentRangeB));
				}
				if ((TimeOfDay.HasValue == true)) {
					command.Parameters[17].Value = ((int)(TimeOfDay.Value));
				}
				else {
					command.Parameters[17].Value = global::System.DBNull.Value;
				}
				if ((LessonPeriod == null)) {
					command.Parameters[18].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[18].Value = ((string)(LessonPeriod));
				}
				if ((Support == null)) {
					command.Parameters[19].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[19].Value = ((string)(Support));
				}
				if ((FileName == null)) {
					command.Parameters[20].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[20].Value = ((string)(FileName));
				}
				if ((BookNo.HasValue == true)) {
					command.Parameters[21].Value = ((int)(BookNo.Value));
				}
				else {
					command.Parameters[21].Value = global::System.DBNull.Value;
				}
				if ((PageNo.HasValue == true)) {
					command.Parameters[22].Value = ((int)(PageNo.Value));
				}
				else {
					command.Parameters[22].Value = global::System.DBNull.Value;
				}
				if ((FormName == null)) {
					command.Parameters[23].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[23].Value = ((string)(FormName));
				}
				if ((FormNo.HasValue == true)) {
					command.Parameters[24].Value = ((int)(FormNo.Value));
				}
				else {
					command.Parameters[24].Value = global::System.DBNull.Value;
				}
				if ((PenId == null)) {
					command.Parameters[25].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[25].Value = ((string)(PenId));
				}
				if ((QASignature.HasValue == true)) {
					command.Parameters[26].Value = ((int)(QASignature.Value));
				}
				else {
					command.Parameters[26].Value = global::System.DBNull.Value;
				}
				if ((StaffSignature.HasValue == true)) {
					command.Parameters[27].Value = ((int)(StaffSignature.Value));
				}
				else {
					command.Parameters[27].Value = global::System.DBNull.Value;
				}
				if ((OverallJudgment.HasValue == true)) {
					command.Parameters[28].Value = ((int)(OverallJudgment.Value));
				}
				else {
					command.Parameters[28].Value = global::System.DBNull.Value;
				}
				if ((OJComments == null)) {
					command.Parameters[29].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[29].Value = ((string)(OJComments));
				}
				if ((DocumentName == null)) {
					command.Parameters[30].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[30].Value = ((string)(DocumentName));
				}
				if ((FollowUp.HasValue == true)) {
					command.Parameters[31].Value = ((bool)(FollowUp.Value));
				}
				else {
					command.Parameters[31].Value = global::System.DBNull.Value;
				}
				if ((DateForFollowUp.HasValue == true)) {
					command.Parameters[32].Value = ((System.DateTime)(DateForFollowUp.Value));
				}
				else {
					command.Parameters[32].Value = global::System.DBNull.Value;
				}
				if ((SummativeFutureDevelopment == null)) {
					command.Parameters[33].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[33].Value = ((string)(SummativeFutureDevelopment));
				}
				if ((SummativeDevelopmentStrategyText == null)) {
					command.Parameters[34].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[34].Value = ((string)(SummativeDevelopmentStrategyText));
				}
				if ((SummativeStrategyBeReviewed == null)) {
					command.Parameters[35].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[35].Value = ((string)(SummativeStrategyBeReviewed));
				}
				if ((SummativeStrategyBeReviewedBy == null)) {
					command.Parameters[36].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[36].Value = ((string)(SummativeStrategyBeReviewedBy));
				}
				if ((TargetsFutureDevelopment == null)) {
					command.Parameters[37].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[37].Value = ((string)(TargetsFutureDevelopment));
				}
				if ((TargetsBeAchieved == null)) {
					command.Parameters[38].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[38].Value = ((string)(TargetsBeAchieved));
				}
				if ((TargetsBeReviewed == null)) {
					command.Parameters[39].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[39].Value = ((string)(TargetsBeReviewed));
				}
				global::System.Data.ConnectionState previousConnectionState = command.Connection.State;
				if (((command.Connection.State & global::System.Data.ConnectionState.Open)
							!= global::System.Data.ConnectionState.Open)) {
					command.Connection.Open();
				}
				object returnValue;
				try {
					returnValue = command.ExecuteScalar();
				}
				finally {
					if ((previousConnectionState == global::System.Data.ConnectionState.Closed)) {
						command.Connection.Close();
					}
				}
				if (((command.Parameters[1].Value == null)
							|| (command.Parameters[1].Value.GetType() == typeof(global::System.DBNull)))) {
					LessonId = new global::System.Nullable<int>();
				}
				else {
					LessonId = new global::System.Nullable<int>(((int)(command.Parameters[1].Value)));
				}
				if (((returnValue == null)
							|| (returnValue.GetType() == typeof(global::System.DBNull)))) {
					return null;
				}
				else {
					return ((object)(returnValue));
				}
			}

			[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[global::System.ComponentModel.Design.HelpKeywordAttribute("vs.data.TableAdapter")]
			public virtual object UpdateRecord(
						global::System.Nullable<int> LessonId,
						string Teacher,
						global::System.Nullable<global::System.DateTime> DateEntered,
						string QAType,
						string ObservationType,
						global::System.Nullable<int> Length,
						string Observer,
						global::System.Nullable<int> YearGroup,
						string ClassCode,
						global::System.Nullable<int> Boys,
						global::System.Nullable<int> Girls,
						string Subject,
						global::System.Nullable<int> SetNo,
						global::System.Nullable<int> SetOf,
						string AssessmentRangeA,
						string AssessmentRangeB,
						global::System.Nullable<int> TimeOfDay,
						string LessonPeriod,
						string Support,
						global::System.Nullable<int> OverallJudgment,
						global::System.Nullable<bool> FollowUp,
						global::System.Nullable<global::System.DateTime> DateForFollowUp,
						global::System.Nullable<bool> Validated) {
				global::System.Data.SqlClient.SqlCommand command = this.CommandCollection[3];
				if ((LessonId.HasValue == true)) {
					command.Parameters[1].Value = ((int)(LessonId.Value));
				}
				else {
					command.Parameters[1].Value = global::System.DBNull.Value;
				}
				if ((Teacher == null)) {
					command.Parameters[2].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[2].Value = ((string)(Teacher));
				}
				if ((DateEntered.HasValue == true)) {
					command.Parameters[3].Value = ((System.DateTime)(DateEntered.Value));
				}
				else {
					command.Parameters[3].Value = global::System.DBNull.Value;
				}
				if ((QAType == null)) {
					command.Parameters[4].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[4].Value = ((string)(QAType));
				}
				if ((ObservationType == null)) {
					command.Parameters[5].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[5].Value = ((string)(ObservationType));
				}
				if ((Length.HasValue == true)) {
					command.Parameters[6].Value = ((int)(Length.Value));
				}
				else {
					command.Parameters[6].Value = global::System.DBNull.Value;
				}
				if ((Observer == null)) {
					command.Parameters[7].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[7].Value = ((string)(Observer));
				}
				if ((YearGroup.HasValue == true)) {
					command.Parameters[8].Value = ((int)(YearGroup.Value));
				}
				else {
					command.Parameters[8].Value = global::System.DBNull.Value;
				}
				if ((ClassCode == null)) {
					command.Parameters[9].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[9].Value = ((string)(ClassCode));
				}
				if ((Boys.HasValue == true)) {
					command.Parameters[10].Value = ((int)(Boys.Value));
				}
				else {
					command.Parameters[10].Value = global::System.DBNull.Value;
				}
				if ((Girls.HasValue == true)) {
					command.Parameters[11].Value = ((int)(Girls.Value));
				}
				else {
					command.Parameters[11].Value = global::System.DBNull.Value;
				}
				if ((Subject == null)) {
					command.Parameters[12].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[12].Value = ((string)(Subject));
				}
				if ((SetNo.HasValue == true)) {
					command.Parameters[13].Value = ((int)(SetNo.Value));
				}
				else {
					command.Parameters[13].Value = global::System.DBNull.Value;
				}
				if ((SetOf.HasValue == true)) {
					command.Parameters[14].Value = ((int)(SetOf.Value));
				}
				else {
					command.Parameters[14].Value = global::System.DBNull.Value;
				}
				if ((AssessmentRangeA == null)) {
					command.Parameters[15].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[15].Value = ((string)(AssessmentRangeA));
				}
				if ((AssessmentRangeB == null)) {
					command.Parameters[16].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[16].Value = ((string)(AssessmentRangeB));
				}
				if ((TimeOfDay.HasValue == true)) {
					command.Parameters[17].Value = ((int)(TimeOfDay.Value));
				}
				else {
					command.Parameters[17].Value = global::System.DBNull.Value;
				}
				if ((LessonPeriod == null)) {
					command.Parameters[18].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[18].Value = ((string)(LessonPeriod));
				}
				if ((Support == null)) {
					command.Parameters[19].Value = global::System.DBNull.Value;
				}
				else {
					command.Parameters[19].Value = ((string)(Support));
				}
				if ((OverallJudgment.HasValue == true)) {
					command.Parameters[20].Value = ((int)(OverallJudgment.Value));
				}
				else {
					command.Parameters[20].Value = global::System.DBNull.Value;
				}
				if ((FollowUp.HasValue == true)) {
					command.Parameters[21].Value = ((bool)(FollowUp.Value));
				}
				else {
					command.Parameters[21].Value = global::System.DBNull.Value;
				}
				if ((DateForFollowUp.HasValue == true)) {
					command.Parameters[22].Value = ((System.DateTime)(DateForFollowUp.Value));
				}
				else {
					command.Parameters[22].Value = global::System.DBNull.Value;
				}
				if ((Validated.HasValue == true)) {
					command.Parameters[23].Value = ((bool)(Validated.Value));
				}
				else {
					command.Parameters[23].Value = global::System.DBNull.Value;
				}
				global::System.Data.ConnectionState previousConnectionState = command.Connection.State;
				if (((command.Connection.State & global::System.Data.ConnectionState.Open)
							!= global::System.Data.ConnectionState.Open)) {
					command.Connection.Open();
				}
				object returnValue;
				try {
					returnValue = command.ExecuteScalar();
				}
				finally {
					if ((previousConnectionState == global::System.Data.ConnectionState.Closed)) {
						command.Connection.Close();
					}
				}
				if (((returnValue == null)
							|| (returnValue.GetType() == typeof(global::System.DBNull)))) {
					return null;
				}
				else {
					return ((object)(returnValue));
				}
			}

			[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[global::System.ComponentModel.Design.HelpKeywordAttribute("vs.data.TableAdapter")]
			[global::System.ComponentModel.DataObjectMethodAttribute(global::System.ComponentModel.DataObjectMethodType.Update, false)]
			public virtual int UpdateValidated(global::System.Nullable<bool> Validated, int LessonId) {
				global::System.Data.SqlClient.SqlCommand command = this.CommandCollection[4];
				if ((Validated.HasValue == true)) {
					command.Parameters[0].Value = ((bool)(Validated.Value));
				}
				else {
					command.Parameters[0].Value = global::System.DBNull.Value;
				}
				command.Parameters[1].Value = ((int)(LessonId));
				global::System.Data.ConnectionState previousConnectionState = command.Connection.State;
				if (((command.Connection.State & global::System.Data.ConnectionState.Open)
							!= global::System.Data.ConnectionState.Open)) {
					command.Connection.Open();
				}
				int returnValue;
				try {
					returnValue = command.ExecuteNonQuery();
				}
				finally {
					if ((previousConnectionState == global::System.Data.ConnectionState.Closed)) {
						command.Connection.Close();
					}
				}
				return returnValue;
			}
		}
	}
}