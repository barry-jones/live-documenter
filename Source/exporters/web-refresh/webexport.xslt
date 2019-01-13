<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
    >
	<xsl:output method="xml" indent="no" />
	<xsl:output omit-xml-declaration="yes" />
	<xsl:key name="itemBykey" match="item" use="concat(@key, '-', @subkey)"/>
	<xsl:variable name="id" select="/*/@id" />
	<xsl:variable name="subId" select="/*/@subId" />
	<xsl:param name="directory" />
	<xsl:variable name="toc" select="concat($directory,'toc.xml')"/>

	<xsl:template match="/frontpage | /members | /member | /namespace | /namespaces">
		<xsl:text disable-output-escaping='yes'>&lt;!DOCTYPE html&gt;</xsl:text>
		<html>
			<head>
				<xsl:call-template name="title" />
				<xsl:call-template name="headreferences" />
			</head>
			<body>
				<div class="container">
					<div class="row">
						<nav class="navigation col-sm-4">
							<div class="brand">
								<img src="styles/images/logo.png" />
							</div>
							<ul>
								<xsl:if test="/frontpage">
									<li>
										<a class="current" href="index.htm">Documentation</a>
									</li>
									<xsl:for-each select="document($toc)/toc/item">
										<xsl:if test="not(@subkey = '')">
											<li>
												<a href="{@key}-{@subkey}.htm">
													<xsl:value-of select="@name"/>
												</a>
											</li>
										</xsl:if>
										<xsl:if test="not(not(@subkey = ''))">
											<li>
												<a href="{@key}.htm">
													<xsl:value-of select="@name"/>
												</a>
											</li>
										</xsl:if>
									</xsl:for-each>
								</xsl:if>
								<xsl:if test="not(/frontpage)">
									<li>
										<a href="index.htm">Documentation</a>
									</li>
								</xsl:if>
							</ul>
							<xsl:if test="/members | /member | /namespace | /namespaces">
								<xsl:apply-templates select="document($toc)" />
							</xsl:if>
						</nav>
						<main class="col-sm-8">
							<div>
								<xsl:if test="/frontpage">
									<xsl:call-template name="front" />
								</xsl:if>
								<xsl:if test="/members">
									<xsl:call-template name="content_members" />
								</xsl:if>
								<xsl:if test="/member">
									<xsl:call-template name="content_member" />
								</xsl:if>
								<xsl:if test="/namespace">
									<xsl:call-template name="content_namespace" />
								</xsl:if>
								<xsl:if test="/namespaces">
									<xsl:call-template name="content_namespaces" />
								</xsl:if>
							</div>
						</main>
					</div>
				</div>
				<xsl:call-template name="footer" />
			</body>
		</html>
	</xsl:template>

	<xsl:template name="front">
		<section class="details">
			<h1>Class Library Documentation</h1>
		</section>
		<section class="namespaces">
			<h2>Namespaces</h2>
			<xsl:apply-templates select="namespaces/namespace" />
		</section>
	</xsl:template>

	<xsl:template name="content_members">
		<xsl:apply-templates select="name" />
		<p>The <xsl:value-of select="name/@type" /> type exposes the following members.</p>
		<xsl:call-template name="member-lists" />
	</xsl:template>

	<xsl:template name="content_namespace">
		<section class="details">
			<h1><xsl:apply-templates select="name" /></h1>
		</section>
		<xsl:if test="count(/namespace/parent[@type='class']) > 0">
			<section class="members classes">
				<h2>Classes</h2>
				<xsl:apply-templates select="/namespace/parent[@type = 'class']" />
			</section>
		</xsl:if>
		<xsl:if test="count(/namespace/parent[@type='structure']) > 0">
			<section class="members structures">
				<h2>Structures</h2>
				<xsl:apply-templates select="/namespace/parent[@type = 'structure']" />
			</section>
		</xsl:if>
		<xsl:if test="count(/namespace/parent[@type='interface']) > 0">
			<section class="members interfaces">
				<h2>Interfaces</h2>
				<xsl:apply-templates select="/namespace/parent[@type = 'interface']" />
			</section>
		</xsl:if>
		<xsl:if test="count(/namespace/parent[@type='delegate']) > 0">
			<section class="members delegates">
				<h2>Delegates</h2>
				<xsl:apply-templates select="/namespace/parent[@type = 'delegate']" />
			</section>	
		</xsl:if>
		<xsl:if test="count(/namespace/parent[@type='enum']) > 0">
			<section class="members enums">
				<h2>Enumerations</h2>
				<xsl:apply-templates select="/namespace/parent[@type = 'enum']" />
			</section>
		</xsl:if>
	</xsl:template>

	<xsl:template name="content_namespaces">
		<section class="details">
			<h1><xsl:apply-templates select="name" /></h1>
		</section>
		<section class="members">
			<xsl:apply-templates select="entry" />
		</section>
	</xsl:template>

	<xsl:template match="/namespaces/entry">
		<div class="entry">
			<a href="{@key}-{@subkey}.htm">
				<xsl:value-of select="@name" />
			</a>
		</div>
	</xsl:template>
	
	<xsl:template name="content_member">
		<section class="details">
			<h1><xsl:value-of select="/member/name" /></h1>
		
			<xsl:apply-templates select="/member/summary" />
			<xsl:apply-templates select="/member/inheritance" />
		
			<xsl:if test="(namespace or assembly)">
				<div class="details">
					<xsl:if test="namespace">
						<span class="namespace">
							<em>Namespace:</em>
							<xsl:text> </xsl:text>
							<a href="{/member/namespace/@id}-{/member/namespace/@name}.htm">
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
		</section>
		
		<xsl:apply-templates select="/member/genericparameters" />
		<xsl:apply-templates select="/member/parameters" />
		<xsl:apply-templates select="/member/return" />
		
		<xsl:apply-templates select="/member/example" />

		<xsl:apply-templates select="/member/remarks" />

		<xsl:apply-templates select="/member/exceptions" />

		<xsl:if test="/member/@type != 'delegate'">
			<xsl:call-template name="member-lists" />
		</xsl:if>
		<xsl:apply-templates select="/member/values" />

		<xsl:apply-templates select="/member/seealsolist" />
	</xsl:template>

	<xsl:template name="headreferences">
		<meta charset="UTF-8" />
		<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.2.1/css/bootstrap.min.css" integrity="sha384-GJzZqFGwb1QTTN6wy59ffF1BuGJpLSa9DkKMp0DgiMDm4iYMj70gZWKYbI706tWS" crossorigin="anonymous" />
		<link href="styles/default.css" type="text/css" rel="stylesheet" />
	</xsl:template>

	<xsl:template name="title">
		<xsl:if test="/frontpage">
		<title>Live Documenter API Documentation</title>
		</xsl:if>
		<xsl:if test="/member">
		<title><xsl:value-of select="/member/name"/></title>
		</xsl:if>
		<xsl:if test="/members">
		<title><xsl:value-of select="/*/name"/></title>
		</xsl:if>
		<xsl:if test="/members">
		<title><xsl:value-of select="/namespace/name"/></title>
		</xsl:if>
		<xsl:if test="/members">
		<title><xsl:value-of select="name"/></title>
		</xsl:if>
	</xsl:template>
	
	<xsl:template name="footer">
		<xsl:text disable-output-escaping='yes'>&lt;script src="styles/site.js"&gt;&lt;/script&gt;</xsl:text>
	</xsl:template>

	<xsl:template name="header">
		<header>
			<xsl:text> </xsl:text>
		</header>
	</xsl:template>

	<xsl:template match="/member/values">
		<section class="members fields">
		<h2>Members</h2>
			<xsl:for-each select="/member/values/value">
				<div class="entry row">
					<div class="col-sm">
						<xsl:value-of select="name" />
					</div>
					<xsl:if test="count(description)">
						<div class="col-sm">
							<xsl:value-of select="description" />
						</div>
					</xsl:if>
				</div>
			</xsl:for-each>
		</section>
	</xsl:template>

	<xsl:template name="member-lists">
		<xsl:if test="count(entries/entry[@type='constructor']) > 0">
			<section class="constructors members">
				<h2>Constructors</h2>
				<xsl:apply-templates select="entries/entry[@type='constructor']" />
			</section>
		</xsl:if>
		<xsl:if test="count(entries/entry[@type='method']) > 0">
			<section class="methods members">
				<h2>Methods</h2>
				<xsl:apply-templates select="entries/entry[@type='method']" />
			</section>
		</xsl:if>
		<xsl:if test="count(entries/entry[@type='properties']) > 0">
			<section class="properties members">
				<h2>Properties</h2>
				<xsl:apply-templates select="entries/entry[@type='properties']" />
			</section>
		</xsl:if>
		<xsl:if test="count(entries/entry[@type='event']) > 0">
			<section class="events members">
				<h2>Events</h2>
				<xsl:apply-templates select="entries/entry[@type='event']" />
			</section>
		</xsl:if>
		<xsl:if test="count(entries/entry[@type='field']) > 0 or count(entries/entry[@type='constant']) > 0">
			<section class="constants members">
				<h2>Fields</h2>
				<xsl:apply-templates select="entries/entry[@type='field' or @type='constant']" />
			</section>
		</xsl:if>
		<xsl:if test="count(entries/entry[@type='operator']) > 0">
			<section class="operators members">
				<h2>Operators</h2>
				<xsl:apply-templates select="entries/entry[@type='operator']" />
			</section>
		</xsl:if>
		<xsl:if test="count(entries/entry[@type='extensionmethod']) > 0">
			<section class="extensionmethods members">
				<h2>Extension Methods</h2>
				<xsl:apply-templates select="entries/entry[@type='extensionmethod']" />
			</section>
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
		<div class="entry row">
			<div class="col-sm">
				<a href="{@id}.htm"><xsl:value-of select="name" /></a>
			</div>
			<xsl:if test="count(summary)">
			<div class="col-sm"> 
				<xsl:apply-templates select="summary" />
			</div>
			</xsl:if>
		</div>
	</xsl:template>

	<xsl:template match="namespace/parent">
		<div class="entry row">
			<div class="col-sm">
				<a href="{@key}.htm">
					<xsl:value-of select="@name" />
				</a>
			</div>
			<xsl:if test="count(summary)">
			<div class="col-sm">
				<xsl:apply-templates select="summary" />
			</div>
			</xsl:if>
		</div>
	</xsl:template>

	<xsl:template match="namespaces/namespace">
		<div class="entry">
			<a href="{@key}-{@subkey}.htm">
				<xsl:value-of select="name" />
			</a>
		</div>
	</xsl:template>

	<xsl:template match="/toc">
		<xsl:for-each select="key('itemBykey',concat($id, '-', $subId))">
			<xsl:call-template name="chain">
				<xsl:with-param name="parents" select="ancestor-or-self::item"/>
				<xsl:with-param name="childs" select="item"/>
			</xsl:call-template>
		</xsl:for-each>
	</xsl:template>
	
	<xsl:template name="chain">
		<xsl:param name="parents"/>
		<xsl:param name="childs"/>
		<xsl:if test="$parents">
			<xsl:variable name="parentCurrent">
				<xsl:if test="concat($parents[1]/@key,$parents[1]/@subkey) = concat($id,$subId)">
					<xsl:text>current</xsl:text>
				</xsl:if>
			</xsl:variable>
			<ul>
				<li>
					<xsl:if test="not($parents[1]/@subkey = '')">
						<a class="{$parentCurrent}" href="{$parents[1]/@key}-{$parents[1]/@subkey}.htm">
							<xsl:value-of select="$parents[1]/@name"/>
						</a>
					</xsl:if>
					<xsl:if test="not(not($parents[1]/@subkey = ''))">
						<a class="{$parentCurrent}" href="{$parents[1]/@key}.htm">
							<xsl:value-of select="$parents[1]/@name"/>
						</a>
					</xsl:if>
					<xsl:call-template name="chain">
						<xsl:with-param name="parents" select="$parents[position()!=1]"/>
						<xsl:with-param name="childs" select="$childs"/>
					</xsl:call-template>
					<xsl:if test="count($parents)=1">
						<ul>
							<xsl:for-each select="$childs">
								<xsl:variable name="current">
									<xsl:if test="concat(@key,@subkey) = concat($id,$subId)">
										<xsl:text>current</xsl:text>
									</xsl:if>
								</xsl:variable>
								<xsl:if test="not(@subkey = '')">
									<li>
										<a class="{$current}" href="{@key}-{@subkey}.htm">
											<xsl:value-of select="@name"/>
										</a>
									</li>
								</xsl:if>
								<xsl:if test="not(not(@subkey = ''))">
									<li>
										<a class="{$current}" href="{@key}.htm">
											<xsl:value-of select="@name"/>
										</a>
									</li>
								</xsl:if>
							</xsl:for-each>
						</ul>
					</xsl:if>
				</li>
			</ul>
		</xsl:if>
	</xsl:template>

	<xsl:template match="/member/inheritance">
		<div class="inheritance">
			<h2>Inheritance</h2>
			<div class="inherits_from">
				<xsl:call-template name="inheritance_item">
					<xsl:with-param name="allitems" select="descendant::type[@current = 'true']/ancestor-or-self::type" />
				</xsl:call-template>
			</div>
			<xsl:if test="count(descendant::type[@current = 'true']/type) > 0">
				<div class="derived">
					<div class="more_items">
						<xsl:call-template name="inheritance_item">
							<xsl:with-param name="allitems" select="descendant::type[@current = 'true']/type" />
						</xsl:call-template>
					</div>
				</div>
			</xsl:if>
		</div>
	</xsl:template>
	
	<xsl:template name="inheritance_item">
		<xsl:param name="allitems" />
		<xsl:for-each select="$allitems">
			<xsl:choose>
				<xsl:when test="@current = 'true'">
					<span class="current">						
						<xsl:call-template name="type-listitem" />
					</span>
				</xsl:when>
				<xsl:otherwise>
					<span>
						<xsl:call-template name="type-listitem" />
					</span>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="type-listitem">
		<xsl:if test="not(@id)">
			<xsl:call-template name="substring-after-last">
				<xsl:with-param name="string" select="@name" />
				<xsl:with-param name="delimiter" select="'.'" />
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="@id">
			<a href="{@id}.htm">
				<xsl:call-template name="substring-after-last">
					<xsl:with-param name="string" select="@name" />
					<xsl:with-param name="delimiter" select="'.'" />
				</xsl:call-template>
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

	<xsl:template match="/member/example">
		<section class="example">
			<h2>Examples</h2>
			<xsl:apply-templates />
		</section>
	</xsl:template>

	<xsl:template match="syntax">
        <div class="syntax">
            <h2>Syntax</h2>
        	<div class="csharp code">
        		<div>C#</div>
            	<pre><xsl:apply-templates /></pre>
        	</div>
        </div>
    </xsl:template>

<xsl:template match="keyword"><span class="keyword"><xsl:apply-templates /></span></xsl:template>

<xsl:template match="text"><xsl:apply-templates /></xsl:template>

	<xsl:template match="remarks">
		<section class="remarks">
			<h2>Remarks</h2>
			<xsl:apply-templates />
		</section>
	</xsl:template>

	<xsl:template match="/member/parameters">
		<section class="parameters members">
			<h3>Parameters</h3>
			<xsl:apply-templates select="parameter" />
		</section>
	</xsl:template>

	<xsl:template match="/member/parameters/parameter">
		<div class="entry row">
			<div class="col-sm">
				<code><xsl:value-of select="@name" /> </code>
				<xsl:if test="not(type[@key])">
					<xsl:value-of select="type" />
				</xsl:if>
				<xsl:if test="type[@key]">
					<a href="{type/@key}.htm">
						<xsl:value-of select="type" />
					</a>
				</xsl:if>
			</div>
			<div class="col-sm">
				<xsl:value-of select="description" />
			</div>
		</div>
	</xsl:template>

	<xsl:template match="/member/genericparameters">
		<section class="genericparameters members">
			<h3>Type Parameters</h3>
			<xsl:apply-templates select="parameter" />
		</section>
	</xsl:template>

	<xsl:template match="/member/genericparameters/parameter">
		<div class="row entry">
			<div class="col-sm">
				<xsl:value-of select="name" />
			</div>
			<div class="col-sm">
				<xsl:value-of select="description" />
			</div>
		</div>
	</xsl:template>
	
	<xsl:template match="return">
		<section class="return">
			<h3>Returns</h3>
			<div class="item">
				<xsl:if test="not(type[@key])">
					<xsl:value-of select="type/@name"/>
				</xsl:if>
				<xsl:if test="type[@key]">
					<a href="{type/@key}.htm">
						<xsl:value-of select="type/@name"/>
					</a>
				</xsl:if>
			</div>
			<div class="returns">
				<xsl:value-of select="returns" />
			</div>
		</section>
	</xsl:template>

	<xsl:template match="exceptions">
		<section class="exceptions members">
			<h3>Exceptions</h3>
			<xsl:apply-templates select="exception" />
		</section>
	</xsl:template>

	<xsl:template match="exception">
		<div class="entry row">
			<div class="col-sm">
				<xsl:if test="name[@key]">
					<a href="{name/@key}.htm">
						<xsl:apply-templates select="name" />
					</a>
				</xsl:if>
				<xsl:if test="not(name[@key])">
					<xsl:apply-templates select="name" />
				</xsl:if>
			</div>
			<div class="col-sm">
				<xsl:apply-templates select="condition" />
			</div>
		</div>
	</xsl:template>

	<xsl:template match="see">
		<xsl:if test="@type='namespace'">
			<a href="{@id}-{@name}.htm">
				<xsl:apply-templates />
			</a>
		</xsl:if>
		<xsl:if test="not(@type) or @type != 'namespace'">
			<xsl:if test="not(@id)">
				<xsl:apply-templates />
			</xsl:if>
			<xsl:if test="@id">
				<a href="{@id}.htm">
					<xsl:apply-templates />
				</a>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="seealsolist">
		<section class="seealso">
			<h2>See also</h2>
			<xsl:apply-templates />
		</section>
	</xsl:template>

	<xsl:template match="seealso">
		<div>
			<xsl:if test="not(@id)">
				<xsl:apply-templates />
			</xsl:if>
			<xsl:if test="@id">
				<a href="{@id}.htm">
					<xsl:apply-templates />
				</a>
			</xsl:if>
		</div>
	</xsl:template>

	<xsl:template match="code" xml:space="preserve">
		<div class="code">
        	<pre><xsl:apply-templates /></pre>
		</div>
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
	
	<xsl:template name="substring-after-last">
		<xsl:param name="string" />
		<xsl:param name="delimiter" />
		<xsl:choose>
			<xsl:when test="contains($string, $delimiter)">
				<xsl:call-template name="substring-after-last">
					<xsl:with-param name="string"
						select="substring-after($string, $delimiter)" />
					<xsl:with-param name="delimiter" select="$delimiter" />
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise><xsl:value-of 
				select="$string" /></xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>