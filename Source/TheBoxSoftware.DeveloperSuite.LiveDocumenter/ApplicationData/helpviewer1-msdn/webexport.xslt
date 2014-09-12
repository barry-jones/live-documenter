<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
    >
	<xsl:output method="xml" indent="no" />
	<xsl:output omit-xml-declaration="yes" />
	<xsl:key name="itemBykey" match="item" use="concat(@key, '-', @subkey)"/>
	<xsl:variable name="id" select="/*/@id" />
	<xsl:variable name="subId" select="/*/@subId" />
	<xsl:param name="directory" />
	<xsl:variable name="toc" select="concat($directory,'toc.xml')"/>

	<xsl:template name="start">
		<xsl:text disable-output-escaping="yes"><![CDATA[<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">]]></xsl:text>
		<xsl:text disable-output-escaping="yes"><![CDATA[<html xmlns="http://www.w3.org/1999/xhtml">]]></xsl:text>
	</xsl:template>
	<xsl:template name="end">
		<xsl:text disable-output-escaping="yes"><![CDATA[</html>]]></xsl:text>
	</xsl:template>

	<xsl:template match="/frontpage">
		<xsl:call-template name="start" />
		<head>
			<title>Class Library Documentation</title>
			<link href="styles/default.css" type="text/css" rel="stylesheet"></link>
			<xsl:call-template name="mshelpviewer1-head">
				<xsl:with-param name="top-level" select="'true'" />
			</xsl:call-template>
		</head>
		<body>
			<div class="content">
				<h1>Class Library Documentation</h1>
				<h2>Namespaces</h2>
				<table>
					<thead>
						<tr>
							<th>Name</th>
							<th>Description</th>
						</tr>
					</thead>
					<xsl:apply-templates select="namespaces/namespace" />
				</table>
			</div>
			<xsl:call-template name="footer" />
		</body>
		<xsl:call-template name="end" />
	</xsl:template>

	<xsl:template match="/members">
		<xsl:call-template name="start" />
		<head>
			<title>
				<xsl:value-of select="/*/name/@safename"/>
			</title>
			<link href="styles/default.css" type="text/css" rel="stylesheet"></link>
			<xsl:call-template name="mshelpviewer1-head" />
		</head>
		<body>
			<div class="content">
				<xsl:apply-templates select="name" />
				<p>
					The <xsl:value-of select="name/@type" /> type exposes the following members.
				</p>
				<xsl:call-template name="member-lists" />
			</div>
			<xsl:call-template name="footer" />
		</body>
		<xsl:call-template name="end" />
	</xsl:template>

	<xsl:template match="/namespaces">
		<xsl:call-template name="start" />
		<head>
			<title>
				<xsl:value-of select="name"/>
			</title>
			<link href="styles/default.css" type="text/css" rel="stylesheet"></link>
			<xsl:call-template name="mshelpviewer1-head" />
		</head>
		<body>
			<div class="content">
				<h1>
					<xsl:apply-templates select="name" />
				</h1>
				<table>
					<thead>
						<tr>
							<th>Namespace</th>
						</tr>
					</thead>
					<tbody>
						<xsl:apply-templates select="entry" />
					</tbody>
				</table>
			</div>
			<xsl:call-template name="footer" />
		</body>
		<xsl:call-template name="end" />
	</xsl:template>

	<xsl:template match="/namespaces/entry">
		<tr>
			<td>
				<a href="ms-xhelp://?id={@key}-{@subkey}">
					<xsl:value-of select="@name" />
				</a>
			</td>
		</tr>
	</xsl:template>

	<xsl:template match="/namespace">
		<xsl:call-template name="start" />
		<head>
			<title>
				<xsl:value-of select="/*/name/@safename"/>
			</title>
			<link href="styles/default.css" type="text/css" rel="stylesheet"></link>
			<xsl:call-template name="mshelpviewer1-head" />
		</head>
		<body>
			<div class="content">
				<h1>
					<xsl:apply-templates select="name" />
				</h1>
				<xsl:if test="count(/namespace/parent[@type='class']) > 0">
					<h2>Classes</h2>
					<table>
						<thead>
							<tr>
								<th class="icon"></th>
								<th>Class</th>
								<th>Summary</th>
							</tr>
						</thead>
						<tbody>
							<xsl:apply-templates select="/namespace/parent[@type = 'class']" />
						</tbody>
					</table>
				</xsl:if>
				<xsl:if test="count(/namespace/parent[@type='structure']) > 0">
					<h2>Structures</h2>
					<table>
						<thead>
							<tr>
								<th class="icon"></th>
								<th>Name</th>
								<th>Summary</th>
							</tr>
						</thead>
						<tbody>
							<xsl:apply-templates select="/namespace/parent[@type = 'structure']" />
						</tbody>
					</table>
				</xsl:if>
				<xsl:if test="count(/namespace/parent[@type='interface']) > 0">
					<h2>Interfaces</h2>
					<table>
						<thead>
							<tr>
								<th class="icon"></th>
								<th>Name</th>
								<th>Summary</th>
							</tr>
						</thead>
						<tbody>
							<xsl:apply-templates select="/namespace/parent[@type = 'interface']" />
						</tbody>
					</table>
				</xsl:if>
				<xsl:if test="count(/namespace/parent[@type='delegate']) > 0">
					<h2>Delegates</h2>
					<table>
						<thead>
							<tr>
								<th class="icon"></th>
								<th>Delegate</th>
								<th>Description</th>
							</tr>
						</thead>
						<tbody>
							<xsl:apply-templates select="/namespace/parent[@type = 'delegate']" />
						</tbody>
					</table>
				</xsl:if>
				<xsl:if test="count(/namespace/parent[@type='enum']) > 0">
					<h2>Enumerations</h2>
					<table>
						<thead>
							<tr>
								<th class="icon"></th>
								<th>Name</th>
								<th>Summary</th>
							</tr>
						</thead>
						<tbody>
							<xsl:apply-templates select="/namespace/parent[@type = 'enum']" />
						</tbody>
					</table>
				</xsl:if>
			</div>
			<xsl:call-template name="footer" />
		</body>
		<xsl:call-template name="end" />
	</xsl:template>

	<xsl:template match="/member">
		<xsl:call-template name="start" />
		<head>
			<title>
				<xsl:value-of select="/*/name/@safename"/>
			</title>
			<link href="styles/default.css" type="text/css" rel="stylesheet"></link>
			<xsl:call-template name="mshelpviewer1-head" />
		</head>
		<body>
			<div class="content">
				<h1>
					<xsl:value-of select="/member/name" />
					<xsl:text> </xsl:text>
					<xsl:call-template name="type-display-name" />
				</h1>

				<xsl:apply-templates select="/member/summary" />

				<xsl:apply-templates select="/member/inheritance" />

				<xsl:if test="(namespace or assembly)">
					<div class="details">
						<xsl:if test="namespace">
							<span class="namespace">
								<em>Namespace:</em>
								<xsl:text> </xsl:text>
								<a href="ms-xhelp://?Id={/member/namespace/@id}-{/member/namespace/@name}">
									<xsl:value-of select="/member/namespace" />
								</a>
							</span>
						</xsl:if>
						<xsl:if test="assembly">
							<span class="assembly">
								<em>Assembly:</em><xsl:text> </xsl:text><xsl:value-of select="/member/assembly" /> in (<xsl:value-of select="/member/assembly/@file" />)
							</span>
						</xsl:if>
					</div>
				</xsl:if>

				<xsl:apply-templates select="/member/*/syntax" />
				<xsl:apply-templates select="/member/genericparameters" />
				<xsl:apply-templates select="/member/parameters" />
				<xsl:apply-templates select="/member/exceptions" />

				<xsl:if test="/member/@type != 'delegate'">
					<xsl:call-template name="member-lists" />
				</xsl:if>
				<xsl:apply-templates select="/member/values" />

				<xsl:apply-templates select="/member/remarks" />
				<xsl:apply-templates select="/member/example" />

				<xsl:apply-templates select="/member/seealsolist" />
			</div>
			<xsl:call-template name="footer" />
		</body>
		<xsl:call-template name="end" />
	</xsl:template>

	<xsl:template name="mshelpviewer1-head">
		<xsl:param name="top-level" />
		<!-- ms help viewer specific meta tags -->
		<xsl:variable name="id" select="@id" />
		<xsl:variable name="subId" select="@subId" />
		<xsl:if test="$top-level = 'true'">
			<meta name="Microsoft.Help.TocParent" content="-1" />
		</xsl:if>
		<xsl:if test="$top-level != 'true'">
			<xsl:for-each select="document($toc)/toc/descendant::*[@key=$id and @subkey=$subId]/parent::node()">
				<xsl:if test="self::toc">
					<meta name="Microsoft.Help.TocParent" content="0" />
				</xsl:if>
				<xsl:if test="not(self::toc)">
					<xsl:if test="not(@subkey = '')">
						<meta name="Microsoft.Help.TocParent" content="{@key}-{@subkey}" />
					</xsl:if>
					<xsl:if test="@subkey = ''">
						<meta name="Microsoft.Help.TocParent" content="{@key}" />
					</xsl:if>
				</xsl:if>
			</xsl:for-each>
		</xsl:if>
		<xsl:if test="not(@subId = '')">
			<meta name="Microsoft.Help.Id" content="{@id}-{@subId}" />
		</xsl:if>
		<xsl:if test="@subId = ''">
			<meta name="Microsoft.Help.Id" content="{@id}" />
		</xsl:if>
		<meta name="Microsoft.Help.SelfBranded" content="true" />
		<meta name="Microsoft.Help.TocOrder" content="1"/>
		<!-- end ms help viewer tags -->
	</xsl:template>

	<xsl:template name="footer">
		<div class="footer">
			Produced by the <a href="http://theboxsoftware.com/products/live-documenter/">Live Documenter</a> developed by <a href="http://theboxsoftware.com">The Box Software</a>.
		</div>
	</xsl:template>

	<xsl:template match="/member/values">
		<h2>Members</h2>
		<table>
			<thead>
				<th>Name</th>
				<th>Description</th>
			</thead>
			<tbody>
				<xsl:for-each select="/member/values/value">
					<tr>
						<td>
							<xsl:value-of select="name" />
						</td>
						<td>
							<xsl:value-of select="description" />
						</td>
					</tr>
				</xsl:for-each>
			</tbody>
		</table>
	</xsl:template>

	<xsl:template name="member-lists">
		<xsl:if test="count(entries/entry) and /member/name != ''">
			<p>
				The <xsl:value-of select="/member/name" /> type exposes the following members.
			</p>
		</xsl:if>
		<xsl:if test="count(entries/entry[@type='constructor']) > 0">
			<h2>Constructors</h2>
			<table>
				<thead>
					<tr>
						<th class="icon"></th>
						<th>Class</th>
						<th>Summary</th>
					</tr>
				</thead>
				<tbody>
					<xsl:apply-templates select="entries/entry[@type='constructor']" />
				</tbody>
			</table>
		</xsl:if>
		<xsl:if test="count(entries/entry[@type='method']) > 0">
			<h2>Methods</h2>
			<table>
				<thead>
					<tr>
						<th class="icon"></th>
						<th>Name</th>
						<th>Summary</th>
					</tr>
				</thead>
				<tbody>
					<xsl:apply-templates select="entries/entry[@type='method']" />
				</tbody>
			</table>
		</xsl:if>
		<xsl:if test="count(entries/entry[@type='properties']) > 0">
			<h2>Properties</h2>
			<table>
				<thead>
					<tr>
						<th class="icon"></th>
						<th>Name</th>
						<th>Summary</th>
					</tr>
				</thead>
				<tbody>
					<xsl:apply-templates select="entries/entry[@type='properties']" />
				</tbody>
			</table>
		</xsl:if>
		<xsl:if test="count(entries/entry[@type='event']) > 0">
			<h2>Events</h2>
			<table>
				<thead>
					<tr>
						<th class="icon"></th>
						<th>Delegate</th>
						<th>Description</th>
					</tr>
				</thead>
				<tbody>
					<xsl:apply-templates select="entries/entry[@type='event']" />
				</tbody>
			</table>
		</xsl:if>
		<xsl:if test="count(entries/entry[@type='field']) > 0 or count(entries/entry[@type='constant']) > 0">
			<h2>Fields</h2>
			<table>
				<thead>
					<tr>
						<th class="icon"></th>
						<th>Name</th>
						<th>Description</th>
					</tr>
				</thead>
				<tbody>
					<xsl:apply-templates select="entries/entry[@type='field' or @type='constant']" />
				</tbody>
			</table>
		</xsl:if>
		<xsl:if test="count(entries/entry[@type='operator']) > 0">
			<h2>Operators</h2>
			<table>
				<thead>
					<tr>
						<th class="icon"></th>
						<th>Name</th>
						<th>Description</th>
					</tr>
				</thead>
				<tbody>
					<xsl:apply-templates select="entries/entry[@type='operator']" />
				</tbody>
			</table>
		</xsl:if>
		<xsl:if test="count(entries/entry[@type='extensionmethod']) > 0">
			<h2>Extension Methods</h2>
			<table>
				<thead>
					<tr>
						<th class="icon"></th>
						<th>Name</th>
						<th>Description</th>
					</tr>
				</thead>
				<tbody>
					<xsl:apply-templates select="entries/entry[@type='extensionmethod']" />
				</tbody>
			</table>
		</xsl:if>
	</xsl:template>

	<xsl:template name="type-display-name">
		<xsl:choose>
			<xsl:when test="@type='class'">Class</xsl:when>
			<xsl:when test="@type='structure'">Structure</xsl:when>
			<xsl:when test="@type='delegate'">Delegate</xsl:when>
			<xsl:when test="@type='enum'">Enumeration</xsl:when>
			<xsl:when test="@type='event'">Event</xsl:when>
			<xsl:when test="@type='interface'">Interface</xsl:when>
			<xsl:when test="@type='assembly'">Assembly</xsl:when>
			<xsl:when test="@type='field'">Field</xsl:when>
			<xsl:when test="@type='constant'">Constant</xsl:when>
			<xsl:when test="@type='operator'">Operator</xsl:when>
			<xsl:when test="@type='method'">Method</xsl:when>
			<xsl:when test="@type='properties'">Property</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="icons">
		<xsl:if test="@visibility='public'">
			<img src="styles/images/vsobject_{@type}.png" alt="public {@type}" />
		</xsl:if>
		<xsl:if test="@visibility != 'public'">
			<img src="styles/images/vsobject_{@type}_{@visibility}.png" alt="{@visibility} {@type}" />
		</xsl:if>
	</xsl:template>

	<xsl:template match="entries/entry">
		<tr>
			<td>
				<xsl:call-template name="icons" />
			</td>
			<td>
				<a href="ms-xhelp://?Id={@id}">
					<xsl:value-of select="name" />
				</a>
			</td>
			<td>
				<xsl:apply-templates select="summary" />
			</td>
		</tr>
	</xsl:template>

	<xsl:template match="namespace/parent">
		<tr>
			<td>
				<xsl:call-template name="icons" />
			</td>
			<td>
				<a href="ms-xhelp://?Id={@key}">
					<xsl:value-of select="@name" />
				</a>
			</td>
			<td>
				<xsl:apply-templates select="summary" />
			</td>
		</tr>
	</xsl:template>

	<xsl:template match="namespaces/namespace">
		<tr>
			<td>
				<a href="ms-xhelp://?id={@key}-{@subkey}">
					<xsl:value-of select="name" />
				</a>
			</td>
			<td>
				<xsl:apply-templates select="summary" />
			</td>
		</tr>
	</xsl:template>

	<xsl:template match="/member/inheritance">
		<div class="inheritance">
			<h2>Inheritance Hierarchy</h2>
			<xsl:apply-templates select="type" />
		</div>
	</xsl:template>

	<xsl:template match="type">
		<ul>
			<li>
				<xsl:if test="@current = 'true'">
					<span class="current">
						<xsl:call-template name="type-listitem" />
					</span>
				</xsl:if>
				<xsl:if test="not(@current)">
					<span>
						<xsl:call-template name="type-listitem" />
					</span>
				</xsl:if>
				<xsl:if test="count(child::type) > 0">
					<xsl:apply-templates select="child::type[position() = 1]" />
				</xsl:if>
			</li>
			<xsl:for-each select="following-sibling::type">
				<li>
					<span>
						<xsl:call-template name="type-listitem" />
					</span>
				</li>
			</xsl:for-each>
		</ul>
	</xsl:template>

	<xsl:template name="type-listitem">
		<xsl:if test="not(@id)">
			<xsl:value-of select="@name" />
		</xsl:if>
		<xsl:if test="@id">
			<a href="ms-xhelp://?Id={@id}">
				<xsl:value-of select="@name" />
			</a>
		</xsl:if>
	</xsl:template>

	<xsl:template match="/members/name">
		<h1>
			<xsl:apply-templates />
		</h1>
	</xsl:template>

	<xsl:template match="/frontpage/title">
		<h1>
			<xsl:apply-templates />
		</h1>
	</xsl:template>

	<xsl:template match="summary">
		<div class="summary">
			<xsl:apply-templates />
		</div>
	</xsl:template>

	<xsl:template match="syntax" xml:space="preserve">
        <div class="syntax">
            <h2>Syntax</h2>
            <pre><xsl:apply-templates /></pre>
        </div>
    </xsl:template>

	<xsl:template match="keyword">
		<span class="keyword">
			<xsl:apply-templates />
		</span>
	</xsl:template>

	<xsl:template match="text">
		<xsl:apply-templates />
	</xsl:template>

	<xsl:template match="remarks">
		<div class="remarks">
			<h2>Remarks</h2>
			<xsl:apply-templates />
		</div>
	</xsl:template>

	<xsl:template match="/member/parameters">
		<div class="parameters">
			<h3>Parameters</h3>
			<dl>
				<xsl:apply-templates select="parameter" />
			</dl>
		</div>
	</xsl:template>

	<xsl:template match="/member/parameters/parameter">
		<dt>
			<xsl:value-of select="@name" />
		</dt>
		<dd>
			<div>
				Type:
				<xsl:if test="not(type[@key])">
					<xsl:value-of select="type" />
				</xsl:if>
				<xsl:if test="type[@key]">
					<a href="{type/@key}.htm">
						<xsl:value-of select="type" />
					</a>
				</xsl:if>
			</div>
			<div>
				<xsl:value-of select="description" />
			</div>
		</dd>
	</xsl:template>

	<xsl:template match="/member/genericparameters">
		<div class="genericparameters">
			<h3>Type Parameters</h3>
			<dl>
				<xsl:apply-templates select="parameter" />
			</dl>
		</div>
	</xsl:template>

	<xsl:template match="/member/genericparameters/parameter">
		<dt>
			<xsl:value-of select="name" />
		</dt>
		<dd>
			<xsl:value-of select="description" />
		</dd>
	</xsl:template>

	<xsl:template match="/member/example">
		<div class="examples">
			<h2>Examples</h2>
			<xsl:apply-templates />
		</div>
	</xsl:template>

	<xsl:template match="exceptions">
		<div class="exceptions">
			<h2>Exceptions</h2>
			<table>
				<thead>
					<tr>
						<th>Exception</th>
						<th>Condition</th>
					</tr>
				</thead>
				<tbody>
					<xsl:apply-templates select="exception" />
				</tbody>
			</table>
		</div>
	</xsl:template>

	<xsl:template match="exception">
		<tr>
			<td>
				<xsl:if test="name[@key]">
					<a href="{name/@key}.htm">
						<xsl:apply-templates select="name" />
					</a>
				</xsl:if>
				<xsl:if test="not(name[@key])">
					<xsl:apply-templates select="name" />
				</xsl:if>
			</td>
			<td>
				<xsl:apply-templates select="condition" />
			</td>
		</tr>
	</xsl:template>

	<xsl:template match="see">
		<xsl:if test="@type='namespace'">
			<a href="ms-xhelp://?Id={@id}-{@name}">
				<xsl:apply-templates />
			</a>
		</xsl:if>
		<xsl:if test="not(@type) or @type != 'namespace'">
			<xsl:if test="not(@id)">
				<xsl:apply-templates />
			</xsl:if>
			<xsl:if test="@id">
				<a href="ms-xhelp://?Id={@id}">
					<xsl:apply-templates />
				</a>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="seealsolist">
		<div class="seealso">
			<h2>See also</h2>
			<xsl:apply-templates />
		</div>
	</xsl:template>

	<xsl:template match="seealso">
		<div>
			<xsl:if test="not(@id)">
				<xsl:apply-templates />
			</xsl:if>
			<xsl:if test="@id">
				<a href="ms-xhelp://?Id={@id}">
					<xsl:apply-templates />
				</a>
			</xsl:if>
		</div>
	</xsl:template>

	<xsl:template match="code" xml:space="preserve">
        <pre class="code"><xsl:apply-templates /></pre>
    </xsl:template>

	<xsl:template match="para">
		<p>
			<xsl:apply-templates />
		</p>
	</xsl:template>

	<xsl:template match="c">
		<span class="code">
			<xsl:apply-templates />
		</span>
	</xsl:template>

	<xsl:template match="list">
		<xsl:if test="@type = 'number'">
			<ol>
				<xsl:apply-templates />
			</ol>
		</xsl:if>
		<xsl:if test="@type != 'number'">
			<ul>
				<xsl:apply-templates />
			</ul>
		</xsl:if>
	</xsl:template>
	<xsl:template match="list/item">
		<li>
			<xsl:apply-templates />
		</li>
	</xsl:template>

	<xsl:template match="table">
		<table>
			<xsl:apply-templates select="header" />
			<xsl:if test="count(row) > 0">
				<tbody>
					<xsl:apply-templates select="row" />
				</tbody>
			</xsl:if>
		</table>
	</xsl:template>
	<xsl:template match="table/header">
		<thead>
			<tr>
				<th>
					<xsl:apply-templates select="cell[1]" />
				</th>
				<th>
					<xsl:apply-templates select="cell[2]" />
				</th>
			</tr>
		</thead>
	</xsl:template>
	<xsl:template match="row">
		<tr>
			<td>
				<xsl:apply-templates select="cell[1]" />
			</td>
			<td>
				<xsl:apply-templates select="cell[2]" />
			</td>
		</tr>
	</xsl:template>
	<xsl:template match="cell">
		<xsl:apply-templates />
	</xsl:template>
</xsl:stylesheet>